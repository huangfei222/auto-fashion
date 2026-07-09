Genesis Engine Naming\_Convention

Document ID： DOC-003

Document Name： Naming\_Convention

Version： 1.0.0

Status： Frozen

Priority： High

Last Updated： 2026-07-07

Depends On：

DOC-001 Architecture Constitution

DOC-002 Project Directory Standard

1\. 文档目的

统一整个项目的命名规范。

包括：

Godot（GDScript）

ASP.NET Core（C#）

PostgreSQL

JSON 配置

Docker

Git

任何新增代码都必须遵守本规范。

2\. 基本原则

命名必须：

清晰

一致

可读

可搜索

不使用缩写（行业通用缩写除外）

禁止：

a

aa

bbb

temp

test

xxx

abc

3\. 类（Class）

统一：

PascalCase

例如：

Player

Monster

MonsterFactory

CombatSystem

InventoryManager

ConfigManager

EventBus

Logger

DatabaseService

禁止：

monsterFactory

monster\_factory

monsterfactory

4\. 接口（Interface）

统一：

I + PascalCase

例如：

IEntity

IFactory

ILogger

ICombatSystem

IConfigProvider

5\. 枚举（Enum）

统一：

PascalCase

枚举值：

PascalCase

例如：

enum ItemQuality

{

&#x20;   Normal,

&#x20;   Rare,

&#x20;   Epic,

&#x20;   Legendary

}

禁止：

ITEM\_RARE

rare

Rare\_Item

6\. 方法（Method）

统一：

PascalCase

必须使用动词。

例如：

LoadConfig()

SavePlayer()

CalculateDamage()

SpawnMonster()

CreateEntity()

ApplyBuff()

SendPacket()

禁止：

Monster()

Data()

Do()

Test()

7\. 变量（Variable）

统一：

camelCase

例如：

playerId

monsterLevel

dropRate

moveSpeed

maxHealth

currentMana

禁止：

PlayerID

player\_ID

Monster\_HP

8\. 常量（Constant）

统一：

PascalCase

例如：

MaxPlayerLevel

DefaultMoveSpeed

MaxInventorySlots

如果语言或框架要求不同，可遵循语言约定，但整个模块必须保持一致。

9\. 私有字段

统一：

下划线 + camelCase

例如：

\_player

\_currentMap

\_inventory

\_logger

10\. 配置文件

统一：

snake\_case

例如：

monster\_table.json

item\_table.json

npc\_table.json

skill\_table.json

map\_table.json

禁止：

MonsterTable.json

monsterTable.json

monster-config.json

11\. 数据库

表

统一：

snake\_case

例如：

player

player\_item

guild\_member

mail

auction

字段

统一：

snake\_case

例如：

player\_id

account\_id

create\_time

update\_time

move\_speed

12\. JSON 字段

统一：

snake\_case

例如：

monster\_id

move\_speed

drop\_table

attack\_speed

13\. 事件（Event）

统一：

On + PascalCase

例如：

OnPlayerLogin

OnMonsterDead

OnItemDrop

OnQuestCompleted

OnLevelUp

OnGuildCreated

禁止：

MonsterDead

player\_login

Event1

14\. Factory

统一：

对象名称 + Factory

例如：

MonsterFactory

ItemFactory

NpcFactory

SkillFactory

MapFactory

15\. Manager

负责管理资源。

统一：

对象名称 + Manager

例如：

ConfigManager

AudioManager

SceneManager

ResourceManager

16\. Service

负责业务能力。

统一：

对象名称 + Service

例如：

CombatService

TradeService

MailService

LoginService

17\. System

负责运行时系统。

统一：

对象名称 + System

例如：

CombatSystem

BuffSystem

AISystem

NavigationSystem

DropSystem

18\. DTO

统一：

对象名称 + Dto

例如：

PlayerDto

LoginRequestDto

LoginResponseDto

ItemDto

19\. 网络协议

统一：

Request

Response

Packet

例如：

LoginRequest

LoginResponse

MovePacket

ChatPacket

AttackPacket

20\. AI 禁止事项

AI 禁止：

新增命名风格。

混用命名风格。

使用拼音。

使用中文命名。

使用无意义缩写。

使用编号命名。

例如：

Manager1

Monster2

TestSkill

AAA

Tmp

21\. 命名审核清单

提交代码前必须确认：

✓ 命名符合规范。

✓ 无拼音。

✓ 无中文。

✓ 无缩写。

✓ 无临时名称。

✓ 同类对象命名一致。

22\. 修改流程

任何命名规范修改：

必须：

ADR

↓

Review

↓

Version Upgrade

不得直接修改。

End of Document

Status：Frozen

