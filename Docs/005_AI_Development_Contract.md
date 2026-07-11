Genesis Engine AI\_Development\_Contract

Document ID： DOC-005

Document Name： AI\_Development\_Contract

Version： 1.0.0

Status： Frozen

Priority： Highest

Last Updated： 2026-07-07

第一章 文档目的

本文件规定所有 AI（包括 ChatGPT、Codex、Claude、Gemini 及未来其他 AI）参与 Genesis Engine 项目开发时必须遵守的统一规则。

本合同的目标是：

保证开发连续性

保证代码一致性

保证架构稳定性

保证 AI 可替换性

任何 AI 在开始工作前必须阅读本文件。

第二章 AI 身份

AI 是项目协作者，不是项目设计者。

AI 的职责：

实现设计

完善实现

修复问题

更新文档

编写测试

AI 不得：

擅自修改架构

擅自增加模块

擅自改变规范

第三章 AI 开发流程（强制）

每次开始开发必须执行：

阅读 AI\_PROJECT\_CONTEXT.md

阅读 AI\_Current\_Status.md

阅读 AI\_Todo.md

阅读相关 Specs 文档

输出设计方案

等待确认（如涉及重大设计）

编写代码

编写测试

更新文档

更新 CHANGELOG

更新 AI\_Current\_Status

不得跳过任何步骤。

第四章 AI 禁止行为

禁止：

修改顶层目录

修改架构分层

修改公共接口

修改命名规范

修改配置规范

绕过 ConfigManager

绕过 Factory

绕过 Logger

跨模块直接调用（允许的性能优化场景除外）

使用硬编码游戏数据

删除已有文档

删除测试

第五章 AI 输出规范

每次任务必须按以下顺序输出：

任务理解

设计说明

修改文件列表

接口变化

风险分析

实现代码

测试方案

文档更新说明

第六章 文档同步

涉及以下内容必须同步更新文档：

新模块

新接口

配置结构变化

网络协议变化

数据库变化

架构变化

开发状态

不得只修改代码而不更新文档。

第七章 测试要求

所有新增模块必须至少包含：

README

Example

Unit Test

复杂模块增加：

Integration Test

Performance Test

未完成测试不得标记为完成。

第八章 提交要求

每次完成任务必须明确：

已完成内容

未完成内容

已知问题

下一步建议

不得回复“完成了”而没有说明。

第九章 架构修改

任何影响以下内容的修改：

Engine

Framework

Module Interface

Config Structure

EventBus

Network Protocol

必须：

ADR

↓

Review

↓

Version Upgrade

不得直接修改。

第十章 AI 连续性原则

AI 不依赖聊天记录。

AI 必须依赖：

Docs

Specs

AI\_CONTEXT

AI\_Current\_Status

AI\_Todo

CHANGELOG

任何重要信息都必须写入上述文件。

第十一章 最终目标

任何新的 AI 在阅读项目文档后，应能够在一天内理解项目并继续开发，而无需依赖历史聊天记录。

End of Document

Status：Frozen

我的建议（非常重要）

到这里，我发现一个可以进一步提高连续性的地方。

你原来的规划里只有 AI\_Current\_Status.md。

我建议增加两个永久文件：

AI\_CONTEXT/

AI\_Decisions.md

专门记录：

今天为什么这样设计。

例如：

2026-07-07

决定：

Framework 不允许直接访问数据库。

原因：

以后方便更换数据库。

影响：

所有数据必须经过 Repository。

以及：

AI\_CONTEXT/

AI\_Known\_Issues.md

记录：

已知 Bug

技术债

暂时不解决的问题

后续优化计划

