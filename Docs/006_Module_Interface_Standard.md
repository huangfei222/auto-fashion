Genesis Engine Module\_Interface\_Standard

Document ID： DOC-006

Document Name： Module\_Interface\_Standard

Version： 1.0.0

Status： Frozen

Priority： Highest

Last Updated： 2026-07-07

Depends On：

DOC-001\_Architecture\_Constitution

DOC-002\_Project\_Directory\_Standard

DOC-003\_Naming\_Convention

DOC-004\_Config\_Specification

DOC-005\_AI\_Development\_Contract

第一章 文档目的

本规范定义 Genesis Engine 所有模块的统一设计标准。

任何模块（Engine、Framework、Game Support）都必须遵循本规范。

目标：

模块可独立开发

模块可独立测试

模块可独立替换

模块可独立维护

AI 可独立理解

第二章 模块定义

模块（Module）是项目中最小的功能单元。

例如：

Logger

EventBus

ConfigManager

Combat

Inventory

Quest

Guild

Chat

模块不是目录，而是具有完整生命周期和职责边界的独立组件。

第三章 单一职责原则

一个模块只能负责一个业务领域。

例如：

Combat：

负责：

战斗计算

伤害计算

攻击判定

战斗事件

不得负责：

掉落

背包

成就

任务

这些功能通过 EventBus 响应。

第四章 模块目录规范

每个模块统一结构：

ModuleName/

README.md

Interface/

Core/

Events/

Config/

Tests/

Example/

推荐扩展：

Docs/

Benchmark/

Mock/

Tools/

所有模块保持一致。

第五章 README 必须包含

每个模块必须提供 README。

至少包含：

模块简介

功能范围

不负责内容

依赖模块

对外接口

配置说明

示例

测试说明

更新记录

第六章 生命周期

所有模块统一生命周期：

Init()

↓

Start()

↓

Update(deltaTime)

↓

Stop()

↓

Destroy()

禁止自行定义生命周期。

第七章 模块依赖规则

允许依赖：

Core

禁止：

循环依赖。

例如：

Combat

↓

Inventory

↓

Quest

↓

Combat

属于非法设计。

第八章 通信规范

跨模块通信统一采用：

EventBus。

允许直接调用的情况：

高频数学计算

ECS 内部组件访问

同一模块内部对象

除此之外：

不得跨模块调用内部实现。

第九章 对外接口

模块只能暴露 Public API。

禁止：

访问其他模块内部变量。

禁止：

修改其他模块状态。

例如：

CombatSystem：

允许：

Attack()

CalculateDamage()

禁止：

inventory.items.clear()

第十章 输入与输出

统一流程：

Input

↓

Validate

↓

Process

↓

Output

↓

Event

模块不得直接影响其他模块。

第十一章 配置访问

模块不得读取配置文件。

统一：

ConfigManager

例如：

ConfigManager.GetMonster()

ConfigManager.GetSkill()

ConfigManager.GetDropTable()

第十二章 数据访问

Framework 不得直接访问数据库。

统一：

Repository（后续实现）。

流程：

Framework

↓

Repository

↓

Database

这样未来可更换 PostgreSQL、SQLite 或其他数据库，而不影响业务层。

第十三章 日志规范

统一：

Logger。

禁止：

print()

Console.WriteLine()

统一：

Logger.Debug()

Logger.Info()

Logger.Warning()

Logger.Error()

Logger.Critical()

第十四章 错误处理

模块不得吞掉异常。

所有异常必须：

记录日志。

返回明确错误信息。

对于可恢复错误：

返回 Result。

对于不可恢复错误：

交由统一异常处理。

第十五章 测试要求

每个模块必须包含：

Unit Test

Example

核心模块增加：

Integration Test

Performance Test

第十六章 模块成熟度

定义模块状态：

Draft

Design

Development

Testing

Stable

Deprecated

Released

AI 修改模块时必须同步更新状态。

第十七章 模块版本

每个模块独立版本：

例如：

Combat：

v0.1

↓

v0.2

↓

v1.0

模块升级不得影响其他模块接口。

若接口变化：

必须：

ADR

↓

版本升级

第十八章 禁止事项

禁止：

修改其他模块内部数据

修改其他模块配置

使用全局变量共享状态

在模块中写具体游戏内容

使用硬编码业务数据

跳过 Factory 创建对象

跳过 ConfigManager 获取配置

第十九章 模块审核清单

提交前必须确认：

✓ 职责单一

✓ 生命周期完整

✓ README 已更新

✓ Example 已提供

✓ Tests 已完成

✓ Logger 已使用

✓ ConfigManager 已使用

✓ EventBus 使用符合规范

✓ 无循环依赖

✓ 无硬编码

第二十章 修改流程

任何模块接口修改：

必须：

ADR

↓

Review

↓

Version Upgrade

不得直接修改。

第二十一章 最终目标

Genesis Engine 的每个模块都应满足：

✓ 可独立理解

✓ 可独立开发

✓ 可独立测试

✓ 可独立发布

✓ 可独立维护

✓ AI 可独立接手

✓ 不依赖聊天记录

✓ 不依赖某一个开发者

End of Document

Status：Frozen

