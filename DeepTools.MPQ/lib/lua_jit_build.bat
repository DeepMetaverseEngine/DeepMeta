@set workPath=../http/res_dev/level_000000
@for /r %workPath%  %%v in (*.lua) do luajit -b %%v %%v