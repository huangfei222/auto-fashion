# AI_TODO

**Project:** Genesis Engine  
**Last Updated:** 2026-07-11  
**Status:** Active

---

## 当前最高优先级 P0
（已完成项列出，后续为待办）

### 已完成
- ✅ AI_CONTEXT
- ✅ Week 01 文档冻结
- ✅ 建立项目目录
- ✅ 初始化 Git 仓库
- ✅ 创建 Docker 开发环境
- ✅ 创建空解决方案
- ✅ 测试项目资源文件自动复制配置

---

## 下一阶段 P1 — Core Engine（已完成项与待办）

### 任务状态总览

#### 已完成（P1）
- ✅ Logger
- ✅ EventBus
- ✅ ConfigManager
- ✅ Factory
- ✅ ResourceManager
- ✅ Engine Bootstrap
- ✅ Entity
- ✅ Component System
- ✅ System Framework
- ✅ Engine Runtime Loop
- ✅ Serialization
- ✅ Persistence System
- ✅ ServiceContainer 依赖注入底层
- ✅ Module 模块化架构

#### 当前状态
**阶段:** Core Validation（进入验证与稳定化）  
**说明:** 核心功能已实现并通过本地构建与运行验证；需补充单元/集成测试与 CI 覆盖。

#### 短期待办（P1 补充）
- ☐ 补充单元测试：`SerializationManager` 回退路径、`PersistenceManager` 保存/加载
- ☐ 在 CI 中加入 `dotnet test` 并通过所有测试
- ☐ 记录并清理所有 `Obsolete` 调用（逐步替换 `new PersistenceManager()`）

---

## 第三阶段 P2 — Engine System（Sprint 3：配置驱动架构，当前进行中）
**Sprint3 目标:** 实现完全配置驱动的服务与模块加载（Task021）

### Task021 Global Configuration Pipeline
- **Task021-1** Genesis.Engine.Config.json 统一配置入口 — 进度：90%（配置文件与解析已就绪，需补全 schema 与验证）
- **Task021-2** EngineBootstrap 移除硬编码，反射加载服务 — 进度：完成主要实现，需清理硬编码残留并完善注册顺序
  - 已完成：ServiceLoader 增强、EngineBootstrap 顺序保障（SerializationManager 在前）
  - 受控项：`TryAutoRegisterSerializers` 使用局部 `#pragma` 抑制可空警告（计划重构）
- **Task021-3** 完整配置驱动启动链路验收 — ⬜ 待验收

### 其他 P2 任务
- ⬜ Task022 Global Resource Pipeline
- ⬜ Task023 Runtime Factory Expansion
- ⬜ Task024 Core Engine v0.3 Freeze

---

## 剩余底层任务
- ☐ Scene
- ☐ Network
- ☐ Time
- ☐ Math

---

## 第四阶段 P3 及以后（框架与游戏内容）
**Framework（P3）**
- □ Combat
- □ Skill
- □ Inventory
- □ Equipment
- □ Buff
- □ AI
- □ Quest
- □ NPC
- □ Guild
- □ Team
- □ Trade
- □ Mail
- □ Chat
- □ Ranking

**Game Content（P4）**
- □ Maps
- □ Monsters
- □ Items
- □ Skills
- □ Story
- □ UI
- □ Audio

**Playable Demo（P5）**
- 目标：完成第一个可玩的原创 2D MMORPG Demo

---

## 当前优先级与短期行动项（明确可执行任务）

### P0 → P1 验证
- 在 CI 中启用：`dotnet build`、`dotnet test`、静态分析（nullable warnings）
  - 目标：把关键测试加入 CI，确保主分支持续绿色

### Task021 完成路径
- 完成 `Genesis.Engine.Config.json` schema 并在 `ConfigManager` 中加入可选验证
- 替换 `EngineBootstrap` 中剩余硬编码实例化为配置驱动加载（ServiceLoader）
- 验收：在无硬编码情况下完成引导并通过集成测试

### 序列化与持久化稳定化
- 为 `SerializationManager` 回退策略编写单元测试（POCO、List<T>、嵌套类型）
- 替换仓库中所有 `new PersistenceManager()` 为容器解析（非破坏性补丁）

### 可空性与代码质量
- 重构 `TryAutoRegisterSerializers`，消除 `#pragma` 抑制并满足编译器可空性检查
- 逐步把 nullable warnings 清零，把关键警告纳入 CI 阻断项

### 文档与发布准备
- 更新 `README`、`UPGRADE.md`、`CHANGELOG`，记录 DI 使用示例与迁移指南
- 准备 发布说明：v0.3 破坏性变更（删除无参构造）与迁移步骤

---

## 规则与流程（必须遵守）
- 完成任务后必须：
  - 更新本文件（AI_TODO）
  - 更新 `AI_CURRENT_STATUS`
  - 更新 `CHANGELOG`
  - 在 PR 中标注变更要点与验证步骤
- **AI 接管规则:** 任何 AI 修改核心架构前必须先阅读 `AI_PROJECT_CONTEXT.md` 与 `AI_CURRENT_STATUS.md` 并确认当前阶段与任务优先级

---

## 责任与沟通
**当前负责人:** fei  
**建议沟通渠道:** 项目仓库 PR 与 Issue；每日站会更新 Task021 进度

---

## 变更记录（本次更新）
- **2026-07-11:** 更新 Task021 进度与当前 Sprint 状态；记录 ServiceLoader 与 SerializationManager 的关键改进与受控项（`#pragma` 抑制）；明确短期 CI 与测试任务清单
