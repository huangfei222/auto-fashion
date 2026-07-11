using System;
using System.Threading;
using System.Threading.Tasks;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Services;
using Genesis.Engine.Core.Config;

namespace Genesis.Engine.Core.Runtime
{
    /// <summary>
    /// RuntimeContext
    /// - 负责运行时主循环（tick）
    /// - 使用 ServiceContainer 安全解析 SystemManager 等可选服务
    /// - 提供 Start/Stop/Update/Dispose 接口
    /// </summary>
    public class RuntimeContext : IDisposable
    {
        private readonly ServiceContainer services;
        private readonly ConfigManager configManager;
        private readonly TimeSpan tickInterval;
        private CancellationTokenSource? cts;
        private Task? loopTask;
        private readonly object sync = new();

        public bool Running { get; private set; }

        public RuntimeContext(ServiceContainer services, ConfigManager configManager)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.configManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
            tickInterval = TimeSpan.FromMilliseconds(16);
        }

        public void Start()
        {
            lock (sync)
            {
                if (Running) return;
                Running = true;
                cts = new CancellationTokenSource();
                loopTask = Task.Run(() => LoopAsync(cts.Token), cts.Token);
                Logger.Info("RuntimeContext: Started runtime loop.");
            }
        }

        private async Task LoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var sw = System.Diagnostics.Stopwatch.StartNew();

                    try
                    {
                        if (services.TryResolve<Genesis.Engine.Core.Runtime.Systems.SystemManager>(out var sysMgr) && sysMgr != null)
                        {
                            sysMgr.Update((float)tickInterval.TotalSeconds);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn($"RuntimeContext: System update error: {ex.Message}");
                    }

                    sw.Stop();
                    var elapsed = sw.Elapsed;
                    var delay = tickInterval - elapsed;
                    if (delay > TimeSpan.Zero)
                    {
                        await Task.Delay(delay, token).ConfigureAwait(false);
                    }
                    else
                    {
                        await Task.Yield();
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Logger.Error($"RuntimeContext: Unhandled loop exception: {ex.Message}");
            }
            finally
            {
                Logger.Info("RuntimeContext: Loop exiting.");
            }
        }

        public void Update(float deltaTime)
        {
            if (!Running) return;

            try
            {
                if (services.TryResolve<Genesis.Engine.Core.Runtime.Systems.SystemManager>(out var sysMgr) && sysMgr != null)
                {
                    sysMgr.Update(deltaTime);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"RuntimeContext: Manual update error: {ex.Message}");
            }
        }

        public void Stop()
        {
            lock (sync)
            {
                if (!Running) return;
                try
                {
                    var localCts = cts;
                    var localTask = loopTask;
                    localCts?.Cancel();
                    if (localTask != null) localTask.Wait(TimeSpan.FromSeconds(2));
                }
                catch (AggregateException) { }
                catch (Exception ex)
                {
                    Logger.Warn($"RuntimeContext: Stop error: {ex.Message}");
                }
                finally
                {
                    try { cts?.Dispose(); } catch { }
                    cts = null;
                    loopTask = null;
                    Running = false;
                    Logger.Info("RuntimeContext: Stopped runtime loop.");
                }
            }
        }


        public void Dispose()
        {
            Stop();
        }
    }
}
