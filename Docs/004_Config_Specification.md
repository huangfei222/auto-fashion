Genesis Engine Config\_Specification

Document ID： DOC-004

Document Name： Config\_Specification

Version： 1.0.0

Status： Frozen

Priority： Highest

Last Updated： 2026-07-07

Depends On：

DOC-001\_Architecture\_Constitution

DOC-002\_Project\_Directory\_Standard

DOC-003\_Naming\_Convention

第一章 文档目的

本规范定义 Genesis Engine 所有配置文件的统一格式。

整个项目所有静态数据必须遵循本规范。

包括：

Monster

NPC

Item

Skill

Buff

Quest

Drop

Map

AI

Dialog

Localization

Dungeon

Equipment

Shop

Craft

Pet

Mount

所有配置统一管理。

禁止自行定义格式。

第二章 数据驱动原则

Engine 不知道任何游戏内容。

Framework 不知道任何具体数据。

Game 只负责配置。

系统运行流程：

Config

↓

ConfigLoader

↓

ConfigManager

↓

Factory

↓

Runtime Entity

↓

Systems

第三章 配置目录

固定目录：

Game/

└── Configs/

&#x20;   ├── Monster/

&#x20;   ├── NPC/

&#x20;   ├── Item/

&#x20;   ├── Skill/

&#x20;   ├── Buff/

&#x20;   ├── Quest/

&#x20;   ├── Drop/

&#x20;   ├── Map/

&#x20;   ├── AI/

&#x20;   ├── Dialog/

&#x20;   ├── Dungeon/

&#x20;   ├── Shop/

&#x20;   ├── Craft/

&#x20;   ├── Localization/

&#x20;   ├── Profession/

&#x20;   ├── Pet/

&#x20;   ├── Mount/

&#x20;   └── World/

不得新增同级目录。

新增配置类型必须通过 ADR。

第四章 文件命名

统一：

snake\_case

例如：

monster\_table.json

item\_table.json

npc\_table.json

skill\_table.json

drop\_table.json

quest\_table.json

第五章 配置版本

所有配置必须包含：

{

&#x20;   "version":1

}

以后升级：

Version 2

Version 3

不得覆盖旧版本。

第六章 配置编码

统一：

UTF-8

统一：

LF

统一：

JSON（开发阶段）

后期可导出：

Binary

MessagePack

FlatBuffers

但 ConfigManager 接口保持不变。

第七章 所有配置必须拥有公共字段

每一个配置对象必须拥有：

id

code

name

description

version

enabled

例如：

{

&#x20;   "id":10001,

&#x20;   "code":"monster\_slime",

&#x20;   "name":"Slime",

&#x20;   "description":"基础训练怪物",

&#x20;   "version":1,

&#x20;   "enabled":true

}

第八章 ID 规范

ID 永远唯一。

推荐分段：

Monster

10000-19999

NPC

20000-29999

Item

30000-39999

Skill

40000-49999

Quest

50000-59999

Map

60000-69999

Buff

70000-79999

Dungeon

80000-89999

System

90000-99999

禁止重复。

禁止修改已发布 ID。

第九章 Monster 配置

必须包含：

id

code

name

level

max\_hp

max\_mp

attack

defense

magic\_attack

magic\_defense

move\_speed

attack\_speed

vision\_range

chase\_range

respawn\_time

ai\_id

drop\_table\_id

skills

model

animation

sound

例如：

{

&#x20;   "id":10001,

&#x20;   "code":"monster\_slime",

&#x20;   "level":1,

&#x20;   "max\_hp":50,

&#x20;   "attack":5,

&#x20;   "move\_speed":2.5,

&#x20;   "drop\_table\_id":100,

&#x20;   "ai\_id":1

}

第十章 Item 配置

必须包含：

id

code

name

item\_type

quality

max\_stack

sell\_price

buy\_price

bind\_type

icon

description

第十一章 Skill 配置

必须包含：

id

code

name

skill\_type

cooldown

cast\_time

mana\_cost

range

target\_type

effect\_ids

animation

sound

第十二章 NPC 配置

必须包含：

id

code

name

npc\_type

dialog\_id

shop\_id

quest\_list

map\_id

position

rotation

第十三章 Quest 配置

必须包含：

id

title

description

accept\_npc

finish\_npc

conditions

rewards

next\_quest

第十四章 Drop 配置

必须包含：

id

items

weight

drop\_mode

min\_count

max\_count

示例：

{

&#x20;   "id":100,

&#x20;   "items":\[

&#x20;       {

&#x20;           "item\_id":30001,

&#x20;           "weight":5000,

&#x20;           "min\_count":1,

&#x20;           "max\_count":1

&#x20;       }

&#x20;   ]

}

第十五章 Map 配置

必须包含：

id

code

name

scene

width

height

music

weather

spawn\_points

teleports

safe\_zone

第十六章 AI 配置

必须包含：

id

ai\_type

vision

attack\_range

patrol\_radius

return\_distance

skill\_order

第十七章 配置引用规则

统一：

ID

例如：

drop\_table\_id

skill\_id

npc\_id

quest\_id

map\_id

禁止：

字符串引用。

禁止：

名称引用。

第十八章 ConfigManager 唯一入口

任何代码：

不得：

读取 JSON。

不得：

读取文件。

统一：

ConfigManager。

例如：

ConfigManager.GetMonster(id)

ConfigManager.GetItem(id)

ConfigManager.GetQuest(id)

ConfigManager.GetSkill(id)

第十九章 AI 禁止事项

AI 不得：

直接解析配置。

修改配置结构。

增加公共字段。

删除公共字段。

改变 ID。

修改目录。

第二十章 验收标准

配置系统必须满足：

✓ 数据驱动

✓ 易扩展

✓ 可版本管理

✓ 可热更新（未来）

✓ AI 可自动生成

✓ AI 可自动校验

✓ 与 Engine 解耦

第二十一章 修改流程

任何配置结构修改：

必须：

ADR

↓

Review

↓

Version Upgrade

不得直接修改。

End of Document

Status：Frozen

