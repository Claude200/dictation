# Dictation
C#编写的,可以给自己听写的小程序
## Dependences
该程序依赖于MySQL,要使用该程序，首先需要下载安装C#访问MySQL数据库的ADO.NET驱动程序：https://dev.mysql.com/downloads/file/?id=405442。
其次，确认你安装了MySQL。你可以下载一个集合工具包，如phpStudy或WampServer.
## Start with a project
将Program.cs的内容复制粘贴到新建项目的Program.cs中。右击项目中的引用，选择添加引用，之后找到你的MySQL驱动安装处，默认路径是C:\Program Files\MySQL\MySQL Connector Net 6.3.8\Assemblies\v2.0，选择MySql.Data.dll，将其打开。
随后启动MySQL，在你安装的集成工具包里面，在test数据库中新建数据表words，该表有两个字段（注意按顺序）:Chinese, French。
使用一个你的MySQL用户，在MysqlConnector对象的声明中，配置你的用户名，库名，密码等。
最后，运行你的项目。
### Some irritating problems:
1)中文乱码问题：
首先，在MySQL命令行中输入use test以转到test数据库，随后使用命令:alter database 数据库名 character set "字符集";以更改数据库字符集，我们只需把字符集改成utf8就可以了。
