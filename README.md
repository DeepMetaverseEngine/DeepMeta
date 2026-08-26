# DeepMeta

[![Release](https://img.shields.io/github/v/release/DeepMetaverseEngine/DeepMeta?color=blue&style=flat-square)](https://github.com/DeepMetaverseEngine/DeepMeta/releases)
[![License](https://img.shields.io/github/license/DeepMetaverseEngine/DeepMeta?style=flat-square)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey?style=flat-square)](https://github.com/DeepMetaverseEngine/DeepMeta/releases/tag/v1.0.0)

> **DeepMeta** 是专为高性能游戏客户端与服务器端架构设计的 CLI 命令行与核心工具箱。

---

## 🚀 快速下载与安装 (Download)

可以直接下载最新版本的 Windows 可执行文件：

| 版本 | 文件类型 | 下载链接 |
| :--- | :--- | :--- |
| **v1.0.0** (最新版) | Windows CLI (`.exe`) | [📥 点击下载 gamecli.exe](https://github.com/DeepMetaverseEngine/DeepMeta/releases/download/v1.0.0/gamecli.exe) |

---

## 💡 快速上手 (Quick Start)

`gamecli.exe` 为独立单文件程序，**放到任意目录执行即可自动初始化整个游戏工程**。

### 1. 下载工具
通过上述表格中的链接下载 `gamecli.exe`，或使用命令行（PowerShell）下载至你想要创建工程的目标目录：
```powershell
Invoke-WebRequest -Uri "[https://github.com/DeepMetaverseEngine/DeepMeta/releases/download/v1.0.0/gamecli.exe](https://github.com/DeepMetaverseEngine/DeepMeta/releases/download/v1.0.0/gamecli.exe)" -OutFile "gamecli.exe"
```
运行后，会自动创建游戏工程。前提条件是在Windows环境，你需要配置好你的.ssh证书。

比如我把`gamecli.exe`放到一个叫`Aserg`的空目录里，运行后会自动创建工程结构和VS工程。
<img width="2066" height="924" alt="image" src="https://github.com/user-attachments/assets/daac2eda-42a5-42a8-926b-6e13b0d7a282" />
<img width="630" height="565" alt="image" src="https://github.com/user-attachments/assets/ab9cc15e-c88d-4b71-a160-eee278022ff4" />

工具会自动帮你创建VisualStudio的SLN工程。
<img width="2036" height="1374" alt="image" src="https://github.com/user-attachments/assets/205241d9-5c6f-4933-8110-9e36ffbccf7d" />

编译工程后，会生成编辑器工程。
<img width="2193" height="1497" alt="image" src="https://github.com/user-attachments/assets/82fadb24-dc9c-4637-8e5d-acdfbc7b57ea" />

开始你的独立游戏之旅吧。


