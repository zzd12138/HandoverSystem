<template>
	<el-form style="height: auto;">
		<el-card class="card-header">
			<template #header>
				<div class="card-header">
					<span>固定事项</span>
				</div>
			</template>
			<el-divider content-position="left" class="items-divider">重点警情</el-divider>
			<div>
				<HandoverItemList @getImportantInfo="getImportantInfo" :key="timer" :num="index+1" :label="labelList[0]"
					:priorityOption="priorityOption" style="width: 85%;height: auto;"></HandoverItemList>
			</div>
			<el-divider content-position="left" class="items-divider">指令单</el-divider>
			<div>
				<HandoverItemList @getImportantInfo="getOrderInfo" :key="timer" :num="index+1" :label="labelList[1]"
					:priorityOption="priorityOption" style="width: 85%;height: auto;"></HandoverItemList>
			</div>
			<el-divider content-position="left" class="items-divider">二号电</el-divider>
			<div>
				<HandoverItemList @getImportantInfo="getNumTwoInfo" :key="timer" :num="index+1" :label="labelList[2]"
					:priorityOption="priorityOption" style="width: 85%;height: auto;"></HandoverItemList>
			</div>
		</el-card>

		<el-card class="box-card" style="margin-top: 10px;">
			<template #header>
				<div class="card-header">
					<span>其他事项</span>
				</div>
			</template>
			<HandoverItemList @getImportantInfo="getOtherInfo" :key="timer" :num="index+1" :label="labelList[3]"
				:priorityOption="priorityOption" style="width: 85%;height: auto;margin-top: 20px;"></HandoverItemList>
		</el-card>
		<div class="button-item">
			<el-button type="primary" class="handover-button" @click="chooseMan">选择交接人</el-button>
		</div>
		<handover-choose-man :dialogFormVisible="dialogFormVisible" :mans="mans"
			@closeChildDialog="updataDialogFormVisible" @submitSuccess="submitSuccess">
		</handover-choose-man>
	</el-form>

</template>

<script>
	import {
		toRaw
	} from "vue"
	import HandoverItemList from "../handover-items/HandoverItemList.vue"
	import HandoverChooseMan from "../handover-items/HandoverChooseMan.vue"
	export default {
		name: "HandoverItems",
		data() {
			return {
				formData: {
					//是否自动交接未完成事项
					isTransmittedUnDoItems: true,
					//接收人
					recipient: '',
					//发起人
					initiator: '',
					importantInfo: [],
					orderInfo: [],
					numTwoInfo: [],
					otherInfo: []
				},

				labelList: ['重点警情', '指令单', '二号电', '其他事项'],
				mans: [],

				//选择人员弹出框显示状态
				dialogFormVisible: false,
				timer: "",
			}
		},
		created() {
			this.getAllUserName()
			this.formData.initiator = localStorage.getItem('user')
		},

		methods: {
			//更新重点警情数据
			getImportantInfo(e) {
				//把被reactive或readonly后的Proxy对象转换为原来的target对象,使得能获取到想要的值
				//e=toRaw(e)

				//每次重点警情中的输入框和选项框还有删除数据时，从子组件更新重点警情数据
				this.formData.importantInfo = e
			},
			//更新指令单数据
			getOrderInfo(e) {
				this.formData.orderInfo = e
			},
			//更新二号电
			getNumTwoInfo(e) {
				this.formData.numTwoInfo = e
			},
			getOtherInfo(e) {
				this.formData.otherInfo = e
			},
			chooseMan() {
				this.dialogFormVisible = true
			},
			//更新交接选择弹出框状态
			updataDialogFormVisible() {
				this.dialogFormVisible = false
			},
			//提交成功后的回调函数
			submitSuccess() {
				//刷新子子组件
				this.timer = new Date().getTime()

				//往上传回调函数
				this.$emit('submitSuccess')
				//location.reload()
			},
			//获取所有用户姓名
			async getAllUserName() {
				const {
					data: res
				} = await this.$http.get('api/getUsers.ashx')
				if (res.code !== 0) return console.log("获取用户失败")
				var arr;
				for (let item of res.data) {
					if (item.userName !== localStorage.getItem('user')) {
						this.mans.push(item.userName)
					}
				}
			}
		},
		components: {
			HandoverItemList,
			HandoverChooseMan
		}
	}
</script>

<style lang="less" scoped>
	.card-header {
		text-align: center;
	}

	.box-card {
		margin-bottom: 20px;
	}

	.button-item {
		display: flex;
		justify-content: center;
		align-items: center;
	}

	.handover-button {
		width: 150px;
		height: 40px;
		font-size: 20px;
	}

	.items-divider {
		margin-bottom: 20px;
		margin-top: 10px;
	}
</style>
