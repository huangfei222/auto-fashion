Genesis Engine Project\_Directory\_Standard

Document ID： DOC-002

Document Name： Project\_Directory\_Standard

Version： 1.0.0

Status： Frozen

Priority： High

Last Updated： 2026-07-07

Depends On：

DOC-000 Project Vision

DOC-001 Architecture Constitution

1\. 目的

本规范定义 Genesis Engine 的唯一标准目录结构。

所有开发人员、AI、自动化工具必须遵守本目录规范。

禁止随意新增顶层目录。

禁止修改已有目录职责。

任何目录结构调整必须通过 ADR。

2\. 顶层目录

项目根目录固定如下：

Genesis/

├── AI\_CONTEXT/

├── Assets/

├── Build/

├── Database/

├── Docker/

├── Docs/

├── Engine/

├── Framework/

├── Game/

├── Roadmap/

├── Scripts/

├── Specs/

├── Tests/

├── Tools/

├── ThirdParty/

├── .editorconfig

├── .gitignore

├── docker-compose.yml

├── README.md

└── LICENSE

顶层目录不得随意增加。

3\. AI\_CONTEXT

作用：

整个项目知识入口。

任何 AI 开发前必须阅读。

目录：

AI\_CONTEXT/

AI\_PROJECT\_CONTEXT.md

AI\_START\_HERE.md

AI\_WORKFLOW.md

AI\_PROMPT\_TEMPLATE.md

AI\_Current\_Status.md

AI\_Todo.md

AI\_Module\_Index.md

职责：

保存项目当前状态。

不得保存源码。

4\. Assets

客户端资源。

目录：

Assets/

Sprites/

Tilesets/

Animations/

UI/

Fonts/

Audio/

Music/

SFX/

Shaders/

仅保存资源。

不得保存程序代码。

5\. Build

编译输出目录。

例如：

Build/

Client/

Server/

Packages/

Release/

Build 目录禁止提交到 Git。

6\. Database

数据库脚本。

目录：

Database/

Schema/

Migration/

Seed/

Backup/

保存：

SQL

迁移脚本

初始化数据

7\. Docker

Docker 配置。

例如：

Docker/

postgres/

server/

redis/

nginx/

所有开发环境统一由 Docker 管理。

8\. Docs

永久文档。

目录：

Docs/

Architecture/

Standards/

ADR/

Manual/

Meeting/

Docs 只保存正式文档。

聊天记录不得保存。

9\. Engine

核心引擎。

固定目录：

Engine/

Core/

Config/

Factory/

Logger/

Event/

Resource/

Time/

Math/

Network/

Scene/

Runtime/

Engine 不允许包含任何游戏内容。

10\. Framework

玩法框架。

目录：

Framework/

Combat/

Skill/

Inventory/

Equipment/

Quest/

Drop/

NPC/

Chat/

Guild/

Trade/

Team/

Dungeon/

Ranking/

Achievement/

Framework 不允许写具体怪物。

11\. Game

游戏内容。

目录：

Game/

Configs/

Maps/

Monsters/

NPC/

Items/

Skills/

Effects/

Story/

Localization/

UI/

Game 不允许修改 Engine。

12\. Roadmap

开发计划。

目录：

Roadmap/

Milestones/

Weekly/

Monthly/

Releases/

保存未来计划。

13\. Scripts

自动化脚本。

例如：

Scripts/

Build/

Deploy/

Generate/

Tools/

禁止业务代码放入 Scripts。

14\. Specs

模块设计文档。

例如：

Specs/

Combat/

Inventory/

Skill/

Quest/

Network/

Database/

每个模块必须拥有：

README

流程图

接口说明

状态图

数据结构

测试说明

15\. Tests

测试。

目录：

Tests/

Unit/

Integration/

Performance/

Regression/

所有公共模块必须提供测试。

16\. Tools

开发工具。

例如：

Tools/

Importer/

Exporter/

Editors/

Generators/

工具必须独立运行。

17\. ThirdParty

第三方依赖。

目录：

ThirdParty/

Licenses/

Packages/

Plugins/

必须保留许可证。

不得修改第三方源码。

18\. 禁止事项

禁止：

新增顶层目录。

跨目录引用资源。

修改目录职责。

混放配置。

混放资源。

混放测试。

混放脚本。

19\. 修改流程

任何目录修改：

必须：

ADR

↓

Review

↓

Version Upgrade

不得直接修改。

20\. 验收标准

项目目录应满足：

✓ 职责单一

✓ 层次清晰

✓ AI 可理解

✓ 易于维护

✓ 可长期扩展

End of Document

Status：Frozen

