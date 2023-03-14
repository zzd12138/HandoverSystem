<template>
	<div>
		<el-container class="index-container">
			<el-header class="index-header">
				<handover-head></handover-head>
			</el-header>
			<el-main class="index-main">
				<el-tabs type="border-card" :tab-position="tabPosition" v-model="activeName" style="height: 100%;">
					<el-tab-pane label="值班交接" name="first" >
						<handover-items @submitSuccess='submitSuccess' class="transition-box"></handover-items>
					</el-tab-pane>
					<el-tab-pane label="我的待办 " name="second">
						<todo-items :key="timer"></todo-items>
					</el-tab-pane>
					<el-tab-pane label="我发起的 " name="third">
						<handover-my :key="timer"></handover-my>
					</el-tab-pane>
				</el-tabs>
			</el-main>
		</el-container>
	</div>
</template>

<script>
	import HandoverItems from "../handover-items/HandoverItems.vue"
	import TodoItems from "../todo-items/TodoItems.vue"
	import HandoverHead from "../handover-head/HandoverHead.vue"
	import HandoverMy from "../handover-my/HandoverMy.vue"
	export default {
		name: "HandoverIndex",
		data() {
			return {
				//标签页分布方式
				tabPosition: 'left',
				timer: "",
				activeName: 'first',
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
			HandoverItems,
			TodoItems,
			HandoverHead,
			HandoverMy
		}
	}
</script>

<style lang="less">
	/* 重写左侧标签样式 */
	.el-tabs__item {
		font-size: 30px !important;
		height: 170px;
		line-height: 170px;
		text-align: center;
	}

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

	.index-main {
		padding: 0;
		background-color: rgb(245, 247, 250);
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

</style>
