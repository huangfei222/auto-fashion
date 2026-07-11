AI\_PROJECT\_CONTEXT

Genesis Engine

Version：1.0.0

Status：Active

Last Updated：2026-07-07

一、项目定义

本项目名称：

Genesis Engine

定位：

一个 Data-Driven MMORPG Engine。

本项目首先开发一款原创 2D MMORPG（传奇 Like）。

未来可以支持更多 MMORPG 或 ARPG 游戏。

Engine 保持通用。

Game 可替换。

二、项目目标

目标不是复制任何已有游戏。

目标是：

开发一个原创、可持续维护、支持长期更新的 MMORPG。

项目采用：

数据驱动（Data Driven）

模块化（Modular）

事件驱动（Event Driven）

Factory 创建对象

Config 驱动内容

三、开发原则

项目最高原则：

Architecture Constitution

AI Development Contract

ADR

Module Standard

所有开发必须遵守以上规范。

四、项目目录

项目固定目录：

Docs/

Engine/

Framework/

Game/

AI\_CONTEXT/

Specs/

Roadmap/

Resources/

Tests/

Tools/

Build/

Scripts/

不得随意修改目录结构。

五、技术栈

客户端：

Godot

服务端：

ASP.NET Core

数据库：

PostgreSQL

缓存：

Redis（后续加入）

容器：

Docker

版本管理：

Git

开发环境：

Windows + WSL2 Ubuntu

六、架构

Engine

↓

Framework

↓

Game

Engine：

负责能力。

Framework：

负责玩法。

Game：

负责内容。

不得跨层。

七、开发状态

当前开发阶段：

Phase 1

Architecture Design

当前状态：

Documentation Freeze

尚未进入 Engine Coding。

八、AI 工作方式

AI 每次开始工作必须：

阅读 AI\_CONTEXT 全部文件

阅读 Docs

阅读当前模块 Spec

阅读 AI\_Current\_Status

阅读 AI\_Todo

输出设计

编写代码

编写测试

更新文档

九、禁止事项

禁止：

修改架构

修改命名规范

绕过 Factory

绕过 ConfigManager

绕过 EventBus

写具体游戏内容到 Engine

十、成功标准

项目成功标准：

长期可维护

AI 可替换

文档完整

架构稳定

可持续扩展

End of File.



