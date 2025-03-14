﻿# 图片助手(ImageAssistant) 隐私声明

本隐私政策披露 IA 收集、使用个人信息的方式及流程。
遵照各应用商店隐私政策指导原则，扩展只申请获取实现功能所需的最小权限，只做必要通讯，只收集必要信息，并在本隐私政策中显式说明。

## 扩展有哪些明确的对外交互请求，哪些数据会被收集？如何使用收集的信息？

1. 请求从服务器获取当前最新版本信息。

> 扩展目前通过Chrome Web Store, Microsoft Edge Sotre, Firefox ADD-ONS分发，部分中国基于Chromium开发的浏览器应用商店更新通常不及时的情况下，版本信息有助于用户及时获取到最新版本。

2. 定期从服务器拉取更新扩展默认抓取图片地址正则替换规则。

> 扩展包含一项把抓取的缩略图转换成大尺寸图片的功能，该操作是通过对抓取的图片地址与已知的规则做匹配并添加替换后的图片地址实现的，扩展包内包含一个附带的默认规则版本随版本发布，服务器的版本由扩展包版本叠加最新修改，用户也能在options页添加自定义规则。

3. 请求本地地址localhost:61257端口，本请求为处于早期验证阶段桌面客户端功能的一部分，提交内容包含主动选择的抓取的图片对应的URL及REFERER等图片信息。

> 这是一个当前还处在初级阶段由本地客户端实现的收藏及下载功能，扩展会自动请求该地址确认本地扩展是否启动以决定是否提供相应的收藏功能，收藏下载功能本是属用户主动操作行为，提交选中图片的URL地址及引用页地址供进一步操作，本地客户端目前不可用。

4. 向各搜索引擎服务器(google, baidu, sogou)发出的请求，用户主动行为，通过构造第三方搜索引擎搜索地址请求用于实现以图搜图、关键字搜索等功能。

> 通过构造各大知名搜索引擎查询URL实现以图搜图，关键字搜索的功能。

5. 多地址提取时异步请求或开独立页面请求用户提供或选择的url地址。

> 多地址提取时获取页面数据以DOM形式分析获取图片元素地址或开窗口请求地址并获取页面的图片元素地址。

## 需要澄清说明的问题。

因扩展本身的功能是图片抓取及下载，为尽可能全面地覆盖已经考虑到的场景，尽可能全面的抓取图片供用户选择下载，扩展在启用状态下会对所有的页面互联网请求进行监听并临时存储在内存变量里，待页面关闭时自动销毁或在用户对页面执行图片提取操作时作为数据来源展现给用户。

上述操作实现主要体现在两个方面。background页会利用chrome提供的接口监听全局http请求，并分析地址是否包含图片地址特征或返回内容content-type是否为图片决定是否把该地址存储在tab相关的变量内，并在用户对该页面执行提取操作时使用，或在该tab关闭时自动销毁；inspect.js对所有url地址都起作用，该脚本会对当前已经加载的html内容以文本正则匹配的方式搜寻图片特征地址，并对页面发出的所有异步请求返回内容作文本正则匹配以搜寻图片特片地址，得到的结果地址会存储在页面临时变量里，当用户对页面发起提取操作时通过```<input />```标签传递到background页及筛选页供用户操作，同样的，页面关闭时，相应的临时变量会自动销毁。

### 我们不会做什么？

我们都不会收集传递其他站点的用户名、密码、信用卡及Cookie等安全隐私信息。

在当前控制权归属情况下，扩展当前没有、今后也不会与任何靠收集用户访问信息作为变现方式的第三方合作变现。

(本地客户端与扩展的交互操作数据说明将在后续推出桌面客户后进行额外说明。)


Version: 2020/04/28
