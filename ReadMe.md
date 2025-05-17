# TconRecipeGenSharp

\[简体中文\] | \[[English](ReadMe_en.md)\]

# 使用方法

拖拽一个或多个输入文件到程序中，程序会自动解析文件，在工作目录下生成对应的数据文件。<br/>
输入文件示例：

```toml
[foo]
# 以下三项为必填项
item = "ns:foo_ingot"
molten = "ns:molten_foo"
material = "ns:foo"
# 以下为可选项
temperature = 1000
time = 100

[fooo]
# ...
```

---