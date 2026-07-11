Genesis Engine EventBus\_Specification

Document ID： DOC-007

Document Name： EventBus\_Specification

Version： 1.0.0

Status： Frozen

Priority： Highest

Last Updated： 2026-07-07

Depends On：

DOC-001\_Architecture\_Constitution

DOC-006\_Module\_Interface\_Standard

第一章 文档目的

本规范定义 Genesis Engine 中 EventBus 的唯一实现规范。

EventBus 是整个引擎唯一允许的跨模块通信机制。

目标：

解耦模块

提高可维护性

提高可扩展性

保证 AI 开发一致性

第二章 使用原则

EventBus 用于发布已经发生的事实（Event）。

例如：

OnMonsterDead

OnItemPicked

OnQuestCompleted

OnPlayerLogin

OnGuildCreated

Event 不表示请求，而表示事件已经发生。

第三章 不适用场景

以下情况不应使用 EventBus：

高频移动同步

寻路计算

碰撞检测

数学运算

ECS 内部组件访问

这些逻辑允许直接调用，以保证性能。

第四章 事件命名规范

统一格式：

On + Object + Action

例如：

OnPlayerLogin

OnMonsterDead

OnItemDropped

OnQuestCompleted

OnPlayerLevelUp

OnEquipmentChanged

禁止：

Dead

Login

Monster

Event1

第五章 Event 数据结构

所有事件统一包含：

event\_id

event\_name

timestamp

source

payload

其中：

event\_id：唯一事件编号

event\_name：事件名称

timestamp：事件时间

source：事件来源模块

payload：业务数据

第六章 Event 生命周期

统一流程：

Emit

↓

EventQueue

↓

Dispatch

↓

Subscriber

↓

Handler

↓

Complete

任何事件都必须经过 Dispatcher。

第七章 发布规则

模块发布事件时：

必须：

保证数据完整

不修改其他模块状态

不等待订阅者返回结果

发布者无需知道谁订阅了事件。

第八章 订阅规则

订阅模块必须：

明确订阅事件名称

能处理重复事件

能处理空数据

能处理未来新增字段

不得依赖事件发布顺序。

第九章 EventQueue

所有事件先进入队列。

EventBus 负责：

排队

调度

分发

未来支持：

延迟事件

定时事件

批量事件

但 v1.0 只实现即时事件。

第十章 事件优先级

定义三个等级：

High

Normal

Low

默认：

Normal。

仅 Engine 内部允许使用 High。

第十一章 错误处理

事件处理失败：

不得影响其他订阅者。

必须：

记录日志

保留上下文

继续处理后续订阅者

不得因为一个 Handler 抛出异常而中断整个事件链。

第十二章 日志

所有事件：

必须记录 Debug 日志。

关键事件：

额外记录 Info 日志。

错误：

记录 Error 日志。

第十三章 禁止事项

禁止：

事件中直接修改其他模块内部数据

Handler 中再次发布相同事件形成无限循环

使用 EventBus 代替普通函数调用

在 Event 中存放巨大对象

Event Payload 应尽量精简。

第十四章 示例流程

怪物死亡：

CombatSystem

↓

Emit(OnMonsterDead)

↓

EventBus

↓

DropSystem

↓

QuestSystem

↓

AchievementSystem

↓

StatisticsSystem

CombatSystem 不知道其他系统存在。

第十五章 验收标准

EventBus 应满足：

✓ 发布者与订阅者解耦

✓ 支持多个订阅者

✓ 一个订阅失败不影响其他订阅

✓ 可记录日志

✓ 可测试

✓ 可扩展

第十六章 修改流程

EventBus 规范修改：

必须：

ADR

↓

Review

↓

Version Upgrade

不得直接修改。

End of Document

Status：Frozen

