[CentOS](http://www.linuxidc.com/topicnews.aspx?tid=14)（Community ENTerprise Operating System）是Linux发行版之一，它是来自于[Red Hat](http://www.linuxidc.com/topicnews.aspx?tid=10) Enterprise Linux依照开放源代码规定释出的源代码所编译而成。由于出自同样的源代码，因此有些要求高度稳定性的服务器以CentOS替代商业版的Red Hat Enterprise Linux使用。

[Ubuntu](http://www.linuxidc.com/topicnews.aspx?tid=2)是一个以桌面应用为主的Linux操作系统，其名称来自非洲南部祖鲁语或豪萨语的“ubuntu”一词（译为吾帮托或乌班图），意思是“人性”、“我的存在是因为大家的存在”，是非洲传统的一种价值观，类似华人社会的“仁爱”思想。Ubuntu基于Debian发行版和GNOME桌面环境，与Debian的不同在于它每6个月会发布一个新版本。Ubuntu的目标在于为一般用户提供一个最新的、同时又相当稳定的主要由自由软件构建而成的操作系统。Ubuntu具有庞大的社区力量，用户可以方便地从社区获得帮助。





如果是安装在服务器上，CentOS好用。如果是用来搞编程开发，日常使用的桌面用途，Ubuntu才好用。



CentOS更加稳定，Ubuntu对新技术支持度更好，我认为这个问题我们不需要考虑。





### 区别

1.centos中新建的非root用户是没有sudo的权限的，如果需要使用sudo权限必须在/etc/sudoers 中加入账户和权限，所以切换到root账号的时候只需要输入：su,加入root账号的密码即可。在Ubuntu中，一般使用sudo+命令，如果是第一次使用会提示输入当前用户的密码（而不是root的密码）
2.在线安装软件中，centos使用的是yum命令，而ubuntu中使用的是apt-get命令。除此之外yum中还有一个从软件源中搜索摸个软件的方法：yum search +软件名
3.centos是来自于redhat，所以centos支持rpm格式的安装，而ubuntu显然是不支持的。
4.毕竟是不同的公司做的不同的发行版，很多配置文件的位置和默认的文件路径都有很大区别，这个需要使用过程中慢慢体会了。

5.Centos是基于Redhat开源构建的，服务器系统用的最多，Ubuntu是程序员开发环境，桌面环境用的最多得。 

6.对于ubuntu而言，就是linux操作系统的具体，而linux对于ubuntu来说就是他的抽象；在linux操作系统中，因为应用程序和管理策略的不同，有多个版本，例如：ubuntu,fedora,redhat,centos等；

7.Linux是开放源代码的，所以网上会出现各种各样的发行版本，Ubuntu Linux就是其中一种。Ubuntu采用Linux内核，图形界面采用GNOME（Kubuntu使用KDE）。简而言之，Linux系统是个统称，它有Red Hat、Debian、Suse、Ubuntu等发行版本，它们都是用的Linux内核，都是Linux系统。 

8.非常多的商业公司部署在生产环境上的服务器都是使用CentOS系统，Centos是从Redhat源代码编译重新发布版，Centos去除很多与服务器功能无关的应用，系统简单但非常稳定，命令行操作可以方便管理系统和应用，并且有帮助文档和社区的支持。

9.Ubuntu系统有着靓丽的用户界面，完善的包管理系统，强大的软件源支持，丰富的技术社区，并且Ubuntu对计算机硬件的支持优于centos和Debian，兼容性强，Ubuntu应用非常多，但是对于服务器操作系统来说，并不需要太多的应用程序，需要的是稳定，操作方便，维护简单的系统。如果你需要在服务器端使用图形界面，Ubuntu是一个不错的选择，你需要注意的是，图形界面占用的内存非常大，而内存越大的vps 价格也越高。





### CentOS被redhat废掉后的选择

2020年12 月 8 日，CentOS 开发团队在其[官博宣布](https://blog.centos.org/2020/12/future-is-centos-stream/)，CentOS 8 将在 2021 年底结束支持，CentOS 7 由于用户基数与用户贡献较多，因此会按照计划维护至生命周期结束即 2024 年 6 月 30 日，接下来一年会把重心放到 CentOS Stream 上。

而CentOS Stream其实是作为redhat linux的上游存在的，而不是redhat linux 的下游。

主要是redhat公司不想让人再免费使用他们的redhat系统。这在一定程度上影响了centos的声誉，对centos造成重大打击。

oracle公司宣称可以使用他们的与redhat基本兼容的ORACLE linux，可是根据ORACLE公司的一贯尿性，和对JAVA的做法，这种选择要谨慎。

好在已经有CentOS的早期开发者开辟了rocky linux， 项目地址 https://github.com/rocky-linux/rocky， 目前还没有成果，敬请期待吧。

不过个人感觉rocky 不一定能成大器，mariadb好像用的人并不多。

centos目前在服务器上的运行数量非常庞大（我所见到的各个厂家基本都在使用centos），都切换到ubuntu也不太可能。

感觉CentOS Stream 应该还是会有挺多人用的吧，毕竟上游也没太大毛病。

对于ubuntu ，我一看到登录后总提示要升级，需要重启系统，我就有点抓狂了，对于服务器来讲重启太不友好了。