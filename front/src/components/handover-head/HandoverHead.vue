<template>
	<div class="topHead">
		<el-dropdown>
			<p class="user">你好,{{user}}
				<el-icon class="el-icon--right">
					<arrow-down />
				</el-icon>
			</p>
			<template #dropdown>
				<el-dropdown-menu>
					<el-dropdown-item @click="logout()">注销</el-dropdown-item>
					<el-dropdown-item @click="modifyPwdDialogVisible=true">修改密码</el-dropdown-item>
				</el-dropdown-menu>
		 </template>
		</el-dropdown>
		<el-link class="backstage" :underline="false" @click="openAdmin">管理员入口</el-link>
		</div>
		<handover-head-modify-pwd :dialogFormVisible="modifyPwdDialogVisible" @closeChildDialog="closePwdDialog" @success="modifyPwdSuccess">	
		</handover-head-modify-pwd>
		<handover-head-admin :dialogFormVisible="adminDialogVisible" @close="closeAdminDialog"></handover-head-admin>
		
		<!-- 下面为通告栏 -->
		<el-divider style="margin-top: 0px;margin-bottom: 0px;"></el-divider>
	<el-card class="card-main" v-loading="loading" element-loading-background="rgba(255, 255, 255, 0)">
		<template #header>
			<div class="card-header">
				<span>通告</span>
				<el-button class="button" style="float: right;margin-right: 20px;" size="small" @click="addNotice">增加通告
				</el-button>
			</div>
		</template>
		<div class="notice-content">
			<el-scrollbar height="100">
				<div v-for="(item,index) in notice" :key="item.key" class="content-items"
					@click="getNoticeDetail(item)">
					<span style="display: inline-block; width: 90%;">{{item.title}}</span>
					<!-- 		 <span style="float: right;">{{parseTime(item.creatime,'{y}-{m}-{d}')}}</span> -->
					<span style="float: right;">{{item.creatime.split(' ')[0]}}</span>
				</div>
			</el-scrollbar>
		</div>
	</el-card>
	<handover-head-add-notice :dialogFormVisible="addDialogVisible" @closeChildDialog="closeChildDialog"
		@addSuccess='refreshNotices'>
	</handover-head-add-notice>
	<handover-head-notice-detail :details="details" :detailDialogVisible="detailDialogVisible"
		@closeDetailDialog="closeDetailDialog" @deleteSuccess='refreshNotices'
		ref="detailDialog">
	</handover-head-notice-detail>
</template>

<script>
	import HandoverHeadAddNotice from './HandoverHeadAddNotice.vue'
	import HandoverHeadNoticeDetail from './HandoverHeadNoticeDetail.vue'
	import HandoverHeadModifyPwd from './HandoverHeadModifyPwd.vue'
	import HandoverHeadAdmin from './HandoverHeadAdmin.vue'
	import {
		ArrowDown
	} from '@element-plus/icons-vue'
	import {
		ElMessage
	} from 'element-plus'
	export default {
		name: "HandoverHead",
		data() {
			return {
				//通告
				notice: [],
				user: "",
				addDialogVisible: false,
				detailDialogVisible: false,
				modifyPwdDialogVisible: false,
				adminDialogVisible: false,
				loading: false,
				details: [],
				title: "注销"
			}
		},
		created() {
			this.user = localStorage.getItem('user')
			this.getNotices()
		},
		methods: {
			addNotice() {
				this.addDialogVisible = true
			},
			//关闭增加通告页面回调函数
			closeChildDialog() {
				this.addDialogVisible = false
			},
			//关闭通告详情页面回调函数
			closeDetailDialog(id) {
				this.detailDialogVisible = false
			},
			//关闭修改密码页面回调函数
			closePwdDialog(){
				this.modifyPwdDialogVisible=false
			},
			//关闭管理员界面回调函数
			closeAdminDialog(){
				this.adminDialogVisible=false
			},
			//修改密码成功回调函数
			modifyPwdSuccess(){
				this.modifyPwdDialogVisible=false
				//注销后回到登陆页面			
                setTimeout(()=>this.logout(), 500 )
				setTimeout(()=>ElMessage("请重新登陆"), 500 )
			},
			async getNotices() {
				//激活加载状态
				this.loading = true
				// 1. 通过组件实例 this 访问到全局挂载的 $http 属性，并发起Ajax 数据请求
				const {
					data: res
				} = await this.$http.get('api/getNotices.ashx')
				// 2. 判断请求是否成功
				if (res.code !== 0) return this.fail("获取通告失败")
				// 3. 将请求到的数据存储到 data 中，供页面渲染期间使用
				this.notice = res.data
				//关闭加载
				this.loading = false
			},
			fail(msg) {
				ElMessage({
					message: msg,
					type: 'error',
				})
			},
			//注销事件
			logout() {
				localStorage.removeItem('user')
				localStorage.removeItem('policeNumber')
				//跳转页面
				this.$router.push('/login')
			},
			//通告点击事件
			getNoticeDetail(item) {
				this.details = item
				this.detailDialogVisible = true
			},
			//刷新通告
			refreshNotices() {
				//刷新公告栏
				this.getNotices()
				//关闭通告详情小窗
				this.detailDialogVisible = false
				//关闭增加通告小窗
				this.addDialogVisible = false
			},
			
			//管理员入口事件
			      openAdmin() {
			        this.$prompt('请输入密码', '提示', {
			          confirmButtonText: '确定',
			          cancelButtonText: '取消'
			        }).then(({ value }) => {
					if(value=='xzga330411'){
						this.adminDialogVisible=true
					}
					else {
						this.fail("密码错误")
					}
			        }).catch(() => {
						
			        });
			      }
		},
		components: {
			HandoverHeadAddNotice,
			HandoverHeadNoticeDetail,
			HandoverHeadModifyPwd,
			HandoverHeadAdmin,
			ArrowDown
		}
	}
</script>

<style lang="less">
	.card-main {
		width: 70%;
		margin: 0 auto;
		height: 149px;
		margin-top: 10px;
	}

	.card-header {
		text-align: center;
		justify-content: center;
		font-size: 20px;
	}

	.el-card__header {
		padding-top: 4px;
		padding-left: 0px;
		padding-right: 0px;
		padding-bottom: 4px;
	}

	.notice-content {
		padding-top: 0;
	}

	.el-card__body {
		padding-top: 4px;
	}

	.content-items:hover {
		background-color: ghostwhite;
	}

	.head-logout:hover {
		color: rgb(64, 158, 255);
		cursor: pointer;
	}

	.backstage {
		 right: 15px;
		 top: 13px;
		float: right;
	}

	.user {
		cursor: pointer;
		float: right;
		display: inline-block;
		margin-right: 40px;
	}

	.example-showcase .el-dropdown-link {
		cursor: pointer;
		color: var(--el-color-primary);
		display: flex;
		align-items: center;
	}
	.topHead{
		width: 100%;
		//background-color: rgb(245, 247, 250);
		background: linear-gradient(to right, rgb(245, 247, 250),white);
		padding: 0px;
		text-align: right;
	}
</style>
