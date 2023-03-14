<template>
	<div>

		<el-container class="index-container">
			<el-header class="index-header">
				<handover-head></handover-head>
			</el-header>
			<el-container>
			<el-aside width="200px" style="index-aside">
						<el-menu default-active="1" style="background: linear-gradient(to bottom, rgb(245, 247, 250),white);">
							<el-menu-item index="1" class="left-menu">
								<router-link to="/index/HandoverItems">值班交接</router-link>
							</el-menu-item>
							<el-menu-item index="2" class="left-menu">
								<router-link to="/index/TodoItems"> 我的待办</router-link>
							</el-menu-item>
							<el-menu-item index="3" class="left-menu">
								<router-link to="/index/HandoverMy">我发起的</router-link>
							</el-menu-item>
						</el-menu>
			</el-aside>
			<el-main class="index-main">
				<div style="background-color: white; padding: 10px;">
					 <transition name="el-zoom-in-top">
					 <router-view></router-view>
					 </transition>
				</div>


				<!-- <el-tabs type="border-card" :tab-position="tabPosition" v-model="activeName" style="height: 100%;">
					<el-tab-pane label="值班交接" name="first" >
						<handover-items @submitSuccess='submitSuccess' class="transition-box"></handover-items>
					</el-tab-pane>
					<el-tab-pane label="我的待办 " name="second">
						<todo-items :key="timer"></todo-items>
					</el-tab-pane>
					<el-tab-pane label="我发起的 " name="third">
						<handover-my :key="timer"></handover-my>
					</el-tab-pane>
				</el-tabs> -->
			</el-main>
		</el-container>
	</el-container>
	</div>
</template>

<script>
// import HandoverItems from "../handover-items/HandoverItems.vue"
// import TodoItems from "../todo-items/TodoItems.vue"
import HandoverHead from "../handover-head/HandoverHead.vue"
// import HandoverMy from "../handover-my/HandoverMy.vue"
export default {
	name: "HandoverIndex",
	data() {
		return {
			//标签页分布方式
			tabPosition: 'left',
			timer: "",
		}
	},
	methods: {
		//提交交接事项成功后的回调函数
		submitSuccess() {
			//刷新子组件
			this.timer = new Date().getTime()
		},
	},
	components: {
		// HandoverItems,
		// TodoItems,
		// HandoverMy,
		HandoverHead,
	}
}
</script>

<style lang="less">
.index-container {
	position: absolute;
	width: 100%;
	height: 100%;
}

.index-header {
	height: 220px;
	background-color: rgb(245, 247, 250);
	padding: 0px;
}
.aside{
	height: calc(100vh - 220px);// 设置左侧 aside 高度
}
.index-main {
	padding: 0;
	background-color: white;
	height: calc(100vh - 220px);// 设置主体 main 高度
}

.el-tabs {
	--el-tabs-header-height: 300px;
}

.el-tabs__content {
	// height: calc(100vh - 110px);
	height: 97%;
	overflow-y: scroll;
	margin: 0;
	padding: 0;
}
.router-link-active {
    text-decoration: none;//去除默认样式
    color: #409EFF;//高亮的颜色
  }
  a{
    text-decoration: none;
    color: grey;
  }
  .left-menu{
	font-size: 30px;
	height: 170px;
	line-height: 170px;
  }
</style>
