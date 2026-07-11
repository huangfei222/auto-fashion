Genesis Engine Engineering\_Coding\_Standard

Document ID： DOC-008

Document Name： Engineering\_Coding\_Standard

Version： 1.0.0

Status： Frozen

Priority： Highest

Last Updated： 2026-07-07

Depends On：

DOC-001\_Architecture\_Constitution

DOC-003\_Naming\_Convention

DOC-006\_Module\_Interface\_Standard

DOC-007\_EventBus\_Specification

第一章 文档目的

本规范定义 Genesis Engine 所有源代码必须遵守的工程标准。

目标：

保证代码一致性

保证长期可维护

保证 AI 输出统一

保证可测试

保证可阅读

第二章 编码原则

遵循以下原则：

KISS（保持简单）

DRY（避免重复）

SOLID（面向对象设计）

YAGNI（不过度设计）

Composition over Inheritance（组合优于继承）

任何新增代码都必须符合上述原则。

第三章 文件规范

一个文件只负责一个主要类型。

建议：

每个 Class 一个文件。

文件命名：

与 Class 完全一致。

例如：

CombatSystem.cs

ConfigManager.cs

MonsterFactory.cs

禁止：

Test.cs

Helper2.cs

TempFile.cs

第四章 类规范

一个类：

只负责一个职责。

建议：

普通类：

≤300 行。

复杂核心类：

≤500 行。

超过限制：

应考虑拆分。

第五章 方法规范

一个方法：

建议：

≤40 行。

绝对：

≤80 行。

参数：

建议：

≤5 个。

超过：

使用 DTO 或参数对象。

第六章 嵌套限制

条件判断：

建议：

≤3 层。

循环：

避免超过 2 层。

复杂逻辑：

拆分私有方法。

第七章 注释规范

注释说明：

为什么（Why）。

而不是：

代码做什么（What）。

坏例子：

// 给玩家加经验

好例子：

// 为避免重复升级，经验计算统一在服务端完成。

第八章 魔法数字

禁止：

damage = 125

正确：

damage = skill.damage

数学常量除外：

Math.PI

第九章 日志规范

统一：

Logger。

日志等级：

Debug

Info

Warning

Error

Critical

禁止：

print()

Console.WriteLine()

第十章 异常处理

禁止：

吞掉异常。

例如：

catch(Exception)

{

}

必须：

记录日志。

保留上下文。

返回明确结果。

第十一章 配置读取

禁止：

直接读取：

JSON

数据库

CSV

统一：

ConfigManager。

第十二章 Factory

对象统一：

Factory 创建。

禁止：

业务代码：

new Monster()

允许：

monsterFactory.Create()

第十三章 EventBus

跨模块：

统一：

EventBus。

允许直接调用：

高频算法

数学计算

ECS 内部组件

第十四章 单元测试

所有公共模块：

必须：

Unit Test。

目标覆盖率：

核心模块 ≥80%。

普通模块 ≥60%。

第十五章 Code Review 清单

提交前必须确认：

✓ 无硬编码。

✓ 无魔法数字。

✓ 无重复代码。

✓ 无循环依赖。

✓ 使用 Logger。

✓ 使用 ConfigManager。

✓ 使用 Factory。

✓ 测试通过。

✓ README 已更新。

✓ CHANGELOG 已更新。

第十六章 AI 输出要求

AI 每次提交代码必须同时提供：

设计说明

接口变化

测试说明

风险分析

后续建议

不得只输出代码。

第十七章 性能原则

优先级：

正确性

可维护性

可测试性

性能

未经性能测试：

不得提前优化。

第十八章 禁止事项

禁止：

超长函数

超长类

深层嵌套

全局共享状态

拼音命名

中文命名

临时代码提交

注释掉的大段废弃代码

第十九章 验收标准

任何代码进入主分支必须满足：

✓ 编译通过

✓ 单元测试通过

✓ 文档同步

✓ 无架构违规

✓ 命名规范正确

✓ Code Review 完成

第二十章 修改流程

任何编码规范修改：

必须：

ADR

↓

Review

↓

Version Upgrade

不得直接修改。

End of Document

Status：Frozen

