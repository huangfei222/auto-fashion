Genesis Engine Development\_Workflow

Document ID： DOC-009

Document Name： Development\_Workflow

Version： 1.0.0

Status： Frozen

Priority： Highest

Last Updated： 2026-07-07

Depends On：

DOC-001\_Architecture\_Constitution

DOC-005\_AI\_Development\_Contract

DOC-008\_Engineering\_Coding\_Standard

第一章 文档目的

本规范定义 Genesis Engine 项目的统一开发流程。

目标：

保持长期连续开发

保证 AI 与开发者工作方式一致

保证代码质量

保证文档同步

保证项目可追溯

第二章 开发原则

任何功能开发都必须遵循：

设计 → 文档 → 开发 → 测试 → 验收 → 合并

禁止直接编写代码后再补文档。

第三章 标准开发流程

每个开发任务必须按以下顺序进行：

阅读 AI\_CONTEXT

阅读 AI\_Current\_Status.md

阅读 AI\_Todo.md

确认当前模块

输出设计方案

审查设计

编写代码

编写测试

更新文档

更新 CHANGELOG

更新 AI\_Current\_Status.md

更新 AI\_Todo.md

提交代码

不得跳过任何步骤。

第四章 Git 分支规范

统一使用以下分支：

main

develop

feature/\*

hotfix/\*

release/\*

说明：

main：稳定版本

develop：日常开发

feature：单个功能

hotfix：紧急修复

release：发布准备

禁止直接向 main 提交代码。

第五章 Commit 规范

统一格式：

type(scope): description

例如：

feat(combat): add damage calculation

fix(network): resolve reconnect issue

docs(core): update EventBus specification

refactor(config): optimize config cache

test(factory): add factory unit tests

支持的类型：

feat

fix

docs

refactor

perf

style

test

chore

第六章 Pull Request（PR）规范

每次合并必须包含：

功能说明

设计原因

测试结果

是否影响接口

是否需要 ADR

是否更新文档

PR 未完成检查不得合并。

第七章 文档同步

任何代码修改必须同步更新：

Specs（如涉及接口）

Docs（如涉及规范）

CHANGELOG.md

AI\_Current\_Status.md

AI\_Todo.md

代码与文档不得长期不一致。

第八章 数据库变更

数据库结构修改必须：

编写 Migration

更新数据库设计文档

测试升级

测试回滚

禁止直接修改生产数据库。

第九章 配置变更

新增配置必须：

更新 Config 文档

提供默认值

保证向后兼容

更新版本号

禁止删除仍在使用的配置字段。

第十章 发布流程

发布流程：

develop

↓

集成测试

↓

release

↓

回归测试

↓

main

↓

Tag Version

↓

生成 Release Notes

↓

发布

每次发布必须生成版本标签（Tag）。

第十一章 回滚流程

发布失败时：

停止新部署

回滚到上一稳定版本

保留日志

分析原因

编写 ADR（如涉及架构）

禁止在生产环境直接修改代码。

第十二章 AI 开发流程

AI 每次开发必须执行：

阅读 AI\_CONTEXT

阅读当前状态

输出设计

输出接口

输出实现

输出测试

更新文档

AI 不得直接输出代码而省略设计。

第十三章 模块开发流程

每个模块至少包含：

Module/

├── README.md

├── Interface/

├── Core/

├── Tests/

├── Example/

└── CHANGELOG.md

模块必须可独立理解和测试。

第十四章 测试流程

所有模块必须完成：

单元测试（Unit Test）

集成测试（Integration Test）

重要模块增加：

性能测试（Performance Test）

未通过测试不得合并。

第十五章 版本管理

版本号采用：

Major.Minor.Patch

例如：

0.1.0

0.2.0

1.0.0

1.1.0

1.1.1

规则：

Major：重大架构升级

Minor：新增功能

Patch：Bug 修复

第十六章 开发节奏

建议采用固定节奏：

每周制定开发目标

每完成一个模块立即测试

每周更新 Roadmap

每月发布一个可运行版本

坚持“小步快跑”，避免长期积压未验证代码。

第十七章 验收标准

每个功能完成后必须满足：

✓ 功能实现完成

✓ 单元测试通过

✓ 集成测试通过

✓ 文档已更新

✓ CHANGELOG 已更新

✓ AI\_Current\_Status 已更新

✓ AI\_Todo 已更新

✓ 无架构违规

第十八章 修改流程

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

