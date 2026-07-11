Genesis Engine Architecture\_Decision\_Record\_Guide

Document ID： DOC-010

Document Name： Architecture\_Decision\_Record\_Guide

Version： 1.0.0

Status： Frozen

Priority： Highest

Last Updated： 2026-07-07

Depends On：

DOC-001\_Architecture\_Constitution

DOC-005\_AI\_Development\_Contract

DOC-009\_Development\_Workflow

第一章 文档目的

Architecture Decision Record（ADR）用于记录所有重要架构决策。

它回答的问题不是：

改了什么？

而是：

为什么这样设计？

任何开发者或 AI，都应该能够通过 ADR 理解每一项重要决策的背景、原因和影响。

第二章 ADR 原则

ADR 一经接受（Accepted），不得直接修改。

如果未来需要改变设计：

必须新增一份 ADR。

不得覆盖历史记录。

每一次架构演进都必须保留完整历史。

第三章 必须编写 ADR 的情况

以下情况必须新增 ADR：

引擎核心架构调整

新增核心模块

删除核心模块

修改公共接口

修改网络协议

修改数据库设计

修改配置结构

修改事件机制

修改存档格式

修改资源加载流程

修改部署方式

引入新的第三方依赖

影响多个模块的重要性能优化

普通 Bug 修复无需编写 ADR。

第四章 ADR 编号规范

统一编号：

ADR-0001

ADR-0002

ADR-0003

...

编号永久唯一。

删除的 ADR 编号不得重复使用。

第五章 ADR 文件命名

格式：

ADR-0001-Use-EventBus.md

ADR-0002-ConfigManager.md

ADR-0003-Factory-System.md

文件名应能准确表达决策主题。

第六章 ADR 生命周期

每份 ADR 必须包含以下状态之一：

Proposed（提议）

Accepted（已接受）

Superseded（已被后续 ADR 替代）

Deprecated（已废弃）

任何 ADR 的状态变更都应记录日期和原因。

第七章 ADR 模板

每份 ADR 必须使用统一模板：

ADR-XXXX

Title：

Status：

Date：

Authors：

Depends On：

Supersedes：

\---

\## Context

描述当前问题。

\---

\## Decision

说明最终决定。

\---

\## Alternatives

列出曾考虑过的方案及未采用原因。

\---

\## Consequences

分析优点。

分析缺点。

分析风险。

\---

\## Implementation Plan

实施步骤。

\---

\## Rollback Plan

如果失败如何回滚。

\---

\## References

相关文档。

禁止随意增删主要章节。

第八章 ADR 编写要求

内容必须：

客观

清晰

可追溯

可验证

不得使用：

模糊描述

主观猜测

无依据结论

第九章 ADR 与文档关系

ADR 不代替规范文档。

规范文档描述：

系统应该怎样工作。

ADR 描述：

为什么选择这种设计。

两者必须保持一致。

第十章 ADR 与版本关系

重要 ADR 被接受后：

必须：

更新相关文档

更新 CHANGELOG

更新 AI\_Current\_Status

必要时提升版本号

第十一章 AI 使用 ADR 规则

AI 开发前必须：

阅读 Architecture Constitution

阅读相关模块文档

阅读关联 ADR

AI 不得忽略已接受的 ADR。

若发现现有设计存在问题：

应提出新的 ADR，而不是直接修改实现。

第十二章 ADR Review

所有 ADR 在 Accepted 前应经过 Review。

Review 内容包括：

是否符合项目目标

是否符合架构原则

是否影响兼容性

是否存在更简单方案

是否会增加长期维护成本

通过 Review 后方可进入 Accepted 状态。

第十三章 回滚原则

如果 ADR 实施失败：

不得删除 ADR。

应：

保留原记录

新增 ADR 说明回滚原因

更新状态为 Superseded 或 Deprecated

历史必须完整保留。

第十四章 ADR 存放位置

统一目录：

Docs/

└── ADR/

&#x20;   ├── ADR-0001-Use-EventBus.md

&#x20;   ├── ADR-0002-ConfigManager.md

&#x20;   ├── ADR-0003-Factory-System.md

不得存放于其他目录。

第十五章 示例

ADR-0001

Title：

Use EventBus as the Only Cross-Module Communication Mechanism

Status：

Accepted

Context：

模块之间直接调用导致耦合严重。

Decision：

统一采用 EventBus 进行跨模块通信。

Alternatives：

模块直接调用 —— 耦合高，不采用。

全局管理器 —— 可维护性差，不采用。

Consequences：

优点：

解耦

易扩展

易测试

缺点：

调试事件链相对复杂

Implementation Plan：

实现 EventBus

所有模块迁移

编写测试

Rollback Plan：

恢复原通信方式，并新增 ADR 记录原因。

第十六章 验收标准

每份 ADR 必须满足：

✓ 编号唯一

✓ 模板完整

✓ 原因明确

✓ 风险分析完整

✓ 回滚方案明确

✓ 已关联相关文档

✓ 可独立阅读理解

第十七章 修改流程

本规范属于冻结文档。

任何修改必须：

ADR

↓

Review

↓

Version Upgrade

不得直接修改。

End of Document

Status：Frozen

