Genesis Engine Project\_Vision

Document ID： DOC-000

Document Name： Project\_Vision

Version： 1.0.0

Status： Frozen（冻结）

Last Updated： 2026-07-07

Priority： Highest

一、项目名称

Genesis Engine

二、项目定位

Genesis Engine 是一个完全原创的数据驱动（Data-Driven）2D MMORPG 游戏开发框架。

本项目由四个层级组成：

Engine（引擎层）

↓

Framework（游戏系统层）

↓

Game Data（配置数据层）

↓

Game Content（游戏内容层）

Engine 不绑定任何具体游戏。

Framework 不绑定任何具体世界观。

Game Data 负责定义游戏规则。

Game Content 负责定义世界。

三、项目目标

本项目的目标不是复刻任何已有商业游戏。

本项目将开发一款拥有经典 MMORPG 核心体验的原创游戏。

主要特色：

简单易上手

装备掉落驱动成长

长期在线养成

自由交易

小团队可维护

AI 协同开发

全部采用开源技术栈

四、第一款游戏目标

第一款游戏代号：

Genesis World

目标：

建立一款原创 2D MMORPG。

包含：

地图探索

怪物战斗

装备掉落

技能成长

Boss 挑战

自由交易

公会系统

组队玩法

世界观、美术、名称、地图、怪物、装备均为原创设计。

五、项目原则

项目遵循以下核心原则：

数据驱动

模块化

可扩展

可维护

AI 可协同

文档优先

长期持续开发

原创设计

不侵犯第三方知识产权

每周保持可运行版本

六、技术路线

客户端：

Godot 4.x

服务器：

ASP.NET Core

数据库：

PostgreSQL

缓存：

Redis（后续）

开发环境：

Docker

WSL2

Ubuntu

版本管理：

Git

代码仓库：

GitHub（或 Gitea）

CI/CD（后续）：

GitHub Actions

七、开发理念

采用渐进式开发。

先完成：

最小可玩版本（MVP）。

之后：

持续迭代。

任何时候：

项目都必须能够运行。

禁止长期处于无法运行状态。

八、AI 协作原则

AI 只是开发者之一。

项目真正的知识来源是：

文档

Git

ADR

Current Status

Module Specs

AI 不允许依赖聊天记录作为项目依据。

所有重要设计必须沉淀为正式文档。

九、连续性原则（最高优先级）

项目必须满足：

即使：

更换 AI

更换开发电脑

更换开发者

整个项目仍然能够继续开发。

任何重要知识不得只存在聊天记录中。

所有知识必须写入项目文档。

十、长期目标

第一阶段：

完成可公开测试 Alpha。

第二阶段：

完成 Beta。

第三阶段：

正式上线。

第四阶段：

持续更新原创内容。

第五阶段：

Engine 支持开发第二款 MMORPG。

十一、成功标准

项目完成时，应满足：

拥有完整 Engine。

拥有完整 Framework。

第一款原创 MMORPG 可以正常运行。

可持续维护。

AI 可继续接手开发。

所有文档完整。

所有架构可追溯。

所有模块均有测试。

所有配置均数据驱动。

十二、项目承诺

Genesis Engine 项目坚持以下原则：

不抄袭。

不侵犯任何知识产权。

尊重开源协议。

保持工程质量。

坚持长期维护。

坚持持续学习。

坚持原创。

坚持完成。

End of Document.

