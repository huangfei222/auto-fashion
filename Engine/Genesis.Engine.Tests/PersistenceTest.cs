using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Genesis.Engine.Core.Runtime.Persistence;
using Genesis.Engine.Core.Services;

public static class PersistenceTest
{
    // 简单测试用 POCO，不依赖引擎内部 SaveData 结构
    public class TestRecord
    {
        public int Id { get; set; }
        public string? Data { get; set; }
    }

    // 入口：优先使用容器中的 PersistenceManager（推荐）
    public static void Run(ServiceContainer services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (services.TryResolve<PersistenceManager>(out var pm) && pm != null)
        {
            RunWithPersistence(pm);
            return;
        }

        // 回退：容器未注册时使用无参构造（会触发 Obsolete 警告）
#pragma warning disable CS0618
        var fallback = new PersistenceManager();
#pragma warning restore CS0618
        RunWithPersistence(fallback);
    }

    private static void RunWithPersistence(PersistenceManager pm)
    {
        try
        {
            var path = "save_test.json";

            // 准备要保存的数据
            var list = new List<TestRecord>
            {
                new TestRecord { Id = 1, Data = "alpha" },
                new TestRecord { Id = 2, Data = "beta" }
            };

            // 同步保存与加载示例（使用泛型 API）
            pm.Save(path, list);
            Console.WriteLine("PersistenceTest: Save Completed");

            var loaded = pm.Load<List<TestRecord>>(path);
            Console.WriteLine($"PersistenceTest: Loaded Count {loaded?.Count ?? 0}");

            // 异步保存/加载示例（可选）
            var asyncPath = "save_test_async.json";
            var saveTask = pm.SaveAsync(asyncPath, list);
            saveTask.Wait();
            Console.WriteLine("PersistenceTest: Async Save Completed");

            var loadTask = pm.LoadAsync<List<TestRecord>>(asyncPath);
            loadTask.Wait();
            var loadedAsync = loadTask.Result;
            Console.WriteLine($"PersistenceTest: Async Loaded Count {loadedAsync?.Count ?? 0}");

            // 清理测试文件（可选）
            TryDelete(path);
            TryDelete(asyncPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PersistenceTest error: {ex.Message}");
        }
    }

    private static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path)) File.Delete(path);
        }
        catch { }
    }
}
