Genesis Engine Architecture\_Constitution

Document ID： DOC-001

Document Name： Architecture\_Constitution

Version： 1.0.0

Status： Frozen（冻结）

Priority： Highest

Last Updated： 2026-07-07

第一章：总则

第一条 项目目标

Genesis Engine 是一个完全原创的 Data-Driven MMORPG Engine。

本项目不是开发某一款游戏。

而是开发：

一个长期维护的 MMORPG 引擎

一个可扩展 Framework

第一款原创 Legend-like MMORPG

以后所有游戏都建立在同一套 Engine 之上。

第二条 架构目标

整个项目必须满足：

可维护

可扩展

可测试

可替换 AI

可长期演进

可持续开发

任何时候都不得为了赶进度而破坏架构。

第二章：开发原则

第三条 文档优先

任何开发之前：

必须先完成设计。

必须先完成文档。

禁止：

直接开始写代码。

第四条 项目连续性（最高原则）

整个项目不得依赖：

某一次聊天

某一个 AI

某一个开发者

项目真正的记忆只有：

Docs

AI\_CONTEXT

Specs

ADR

Current Status

Git

任何重要知识必须写入文档。

第五条 AI 可替换原则

任何 AI 都必须能够：

一天内接手项目。

AI 开发流程固定：

阅读 Docs

阅读 AI\_CONTEXT

阅读 Current Status

阅读 Todo

阅读 Module Specs

输出设计

编写代码

更新文档

更新 Current Status

更新 Changelog

不得跳过。

第六条 数据驱动

程序负责：

能力（Capability）

配置负责：

内容（Content）

例如：

Monster

Item

NPC

Skill

Quest

Drop

Map

全部来自配置。

程序不得写死游戏内容。

第七条 Engine 与 Game 分离

整个项目固定四层：

Engine

↓

Framework

↓

GameData

↓

GameContent

Engine 永远不知道：

世界观。

怪物名称。

地图名称。

职业名称。

Game 不允许修改 Engine。

第八条 模块独立

每个系统必须独立维护。

例如：

Combat

Inventory

Equipment

Quest

Guild

Trade

Mail

Chat

AI

Navigation

Weather

模块之间：

不得直接依赖实现。

第九条 统一对象创建

业务代码不得随意创建：

Monster

Item

NPC

Skill

统一通过：

Factory 或 Creator 创建。

第十条 配置统一管理

所有静态配置：

统一由 ConfigManager 管理。

任何代码：

不得直接读取：

JSON

CSV

Resource

统一调用：

ConfigManager。

第十一条 事件驱动

跨模块通信：

统一采用 EventBus。

例如：

MonsterDead

ItemDrop

QuestComplete

GuildCreate

PlayerLevelUp

高频逻辑：

允许直接调用。

例如：

移动。

碰撞。

AI Tick。

寻路。

第十二条 禁止硬编码

程序中不得出现：

经验值。

攻击力。

掉率。

等级。

金币。

装备属性。

全部来自配置。

数学常量除外。

第十三条 日志统一

任何模块：

禁止：

print()

Console.WriteLine()

统一：

Logger。

日志等级：

Debug

Info

Warning

Error

Critical

第十四条 模块生命周期

所有模块统一生命周期：

Init()

↓

Start()

↓

Update()

↓

Stop()

↓

Destroy()

不得自行定义生命周期。

第十五条 测试优先

每完成一个模块。

必须提供：

README

Example

Unit Test

接口说明

否则：

不得进入下一模块。

第十六条 版本冻结

任何正式版本：

不得直接修改。

修改流程：

ADR

↓

Review

↓

Version Upgrade

例如：

v1.0

↓

ADR-0003

↓

v1.1

不得：

直接覆盖。

第十七条 渐进式开发

先完成：

可玩的最小版本。

之后：

不断增加：

地图。

怪物。

装备。

Boss。

副本。

活动。

公会。

攻城。

不得：

一次开发全部系统。

第十八条 不过度设计

任何新增架构。

必须回答：

是否真正需要？

是否增加维护成本？

如果：

当前没有需求。

不得提前设计。

坚持：

Keep It Simple。

第三章：项目纪律

任何开发人员（包括 AI）必须遵守：

不得：

修改架构。

不得：

修改目录。

不得：

修改命名规范。

不得：

绕过 ConfigManager。

不得：

绕过 Factory。

不得：

绕过 Logger。

不得：

绕过 EventBus（适用于跨模块）。

不得：

提交未经测试代码。

不得：

提交无文档代码。

第四章：最终目标

Genesis Engine 最终应达到：

✓ 数据驱动

✓ 模块化

✓ AI 可协同

✓ AI 可替换

✓ 长期维护

✓ 原创设计

✓ 开源技术栈

✓ 商业级工程质量

✓ 第一款原创 MMORPG 成功上线

第五章：宪法修改规则

本文件属于：

最高级文档。

任何修改：

必须：

新增 ADR。

评审通过。

升级版本。

例如：

Architecture Constitution

v1.0

↓

ADR-0008

↓

v1.1

禁止：

直接修改。

End of Document

Status：Frozen

