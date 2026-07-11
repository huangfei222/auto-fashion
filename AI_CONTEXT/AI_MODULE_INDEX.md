AI\_MODULE\_INDEX

Project：Genesis Engine

Version：1.0.0

Status：Active

Last Updated：2026-07-07

文档说明

本文件记录 Genesis Engine 所有模块的状态。

任何新增模块必须先登记，再开发。

任何 AI 开发前必须阅读本文件。

Engine Layer

Module

Status

Version

Depends On

Logger

Planned

0.1.0

None

EventBus

Planned

0.1.0

Logger

ConfigManager

Planned

0.1.0

Logger

ResourceManager

Planned

0.1.0

Logger

Factory

Planned

0.1.0

ConfigManager

Entity

Planned

0.1.0

Factory

ECS

Planned

0.1.0

Entity

Scene

Planned

0.1.0

ECS

Network

Planned

0.1.0

EventBus

Save

Planned

0.1.0

ConfigManager

Time

Planned

0.1.0

None

Math

Planned

0.1.0

None

Framework Layer

Module

Status

Combat

Planned

Skill

Planned

Buff

Planned

Inventory

Planned

Equipment

Planned

Quest

Planned

Drop

Planned

NPC

Planned

AI

Planned

Guild

Planned

Team

Planned

Trade

Planned

Mail

Planned

Chat

Planned

Dungeon

Planned

Ranking

Planned

Achievement

Planned

Game Layer

Module

Status

Configs

Planned

Maps

Planned

Monsters

Planned

NPC

Planned

Items

Planned

Skills

Planned

UI

Planned

Audio

Planned

Story

Planned

开发状态定义

Planned：未开始

Design：设计中

Developing：开发中

Testing：测试中

Completed：完成

Deprecated：废弃

更新规则

新增模块：

更新本文件

更新 AI\_TODO

更新 AI\_CURRENT\_STATUS

编写 README

创建 Specs 文档

不得跳过。

End of File.

AI\_CHANGE\_RULES.md

写作

AI\_CHANGE\_RULES

Project：Genesis Engine

Version：1.0.0

Status：Frozen

目的

本文件定义 AI 在开发过程中允许修改与禁止修改的内容。

任何 AI 必须遵守。

永久禁止修改（Protected）

以下内容未经 ADR 不得修改：

Docs/DOC-001 ～ DOC-010

Engine 总体架构

Engine / Framework / Game 三层结构

命名规范

模块边界

EventBus 通信原则

ConfigManager 数据入口

Factory 创建原则

可以修改（Mutable）

以下内容允许在正常开发中修改：

模块实现代码

单元测试

示例代码

README

Game 配置

游戏资源

AI\_TODO

AI\_CURRENT\_STATUS

CHANGELOG

必须经过 ADR 的修改

以下变更必须先新增 ADR：

新增 Engine 核心模块

删除核心模块

修改公共接口

修改配置格式

修改网络协议

修改数据库结构

修改事件机制

修改对象生命周期

AI 工作规则

每次开发必须：

阅读 AI\_PROJECT\_CONTEXT

阅读 AI\_CURRENT\_STATUS

阅读 AI\_TODO

阅读 AI\_MODULE\_INDEX

输出设计

编码

测试

更新文档

验收

若 AI 未遵守本规则：

不得提交代码。

End of File.

AI\_STARTUP\_CHECKLIST.md

\# AI\_STARTUP\_CHECKLIST

每次 AI 开始工作必须完成：

□ 阅读 AI\_PROJECT\_CONTEXT

□ 阅读 AI\_CURRENT\_STATUS

□ 阅读 AI\_TODO

□ 阅读 AI\_MODULE\_INDEX

□ 阅读相关 Specs

□ 阅读相关 ADR

□ 阅读 CHANGELOG

□ 确认当前模块

□ 输出设计方案

□ 确认不会违反 DOC-001～DOC-010

==============================

开发完成必须确认：

□ 编译通过

□ 单元测试通过

□ 文档更新

□ CHANGELOG 更新

□ AI\_CURRENT\_STATUS 更新

□ AI\_TODO 更新

□ AI\_SESSION\_LOG 新增记录

□ Git Commit

==============================

未完成以上检查，不得结束本次开发。

AI\_SESSION\_TEMPLATE.md

\# AI Session Log

Date：

Developer：

AI：

Session：

\--------------------------------

Current Module：

\--------------------------------

Task：

\--------------------------------

Files Changed：

\--------------------------------

Design Summary：

\--------------------------------

Implementation Summary：

\--------------------------------

Tests：

\--------------------------------

Problems：

\--------------------------------

Solutions：

\--------------------------------

ADR：

None

\--------------------------------

Next Step：

\--------------------------------

Duration：

\--------------------------------

Status：

Completed / Interrupted

AI\_CONTEXT/README.md

\# AI\_CONTEXT

本目录是 Genesis Engine 项目的 AI 工作入口。

任何 AI、任何开发者开始工作前，都必须先阅读本目录。

推荐阅读顺序：

1\. AI\_PROJECT\_CONTEXT.md

2\. AI\_CURRENT\_STATUS.md

3\. AI\_TODO.md

4\. AI\_MODULE\_INDEX.md

5\. AI\_CHANGE\_RULES.md

6\. AI\_STARTUP\_CHECKLIST.md

7\. 最新 AI\_SESSION\_LOG

随后根据当前开发模块：

\- 阅读对应 Specs

\- 阅读相关 ADR

\- 阅读 CHANGELOG

完成以上步骤后，方可开始开发。

本目录中的动态文件（AI\_CURRENT\_STATUS、AI\_TODO、AI\_MODULE\_INDEX、AI\_SESSION\_LOG）必须保持最新状态。

它们是保证项目在更换 AI、暂停开发或多人协作时仍能保持连续性的核心。



