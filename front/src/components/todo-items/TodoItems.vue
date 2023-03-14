<template>
	<el-table :data="formData" style="margin: auto;" align="center" border height="auto" width="auto"
		:cell-class-name="delLine" :row-class-name="backColor" :default-sort="{ prop: 'priority', order: 'descending' }"
		v-loading="loading" element-loading-background="rgba(255, 255, 255, 0)">
		<el-table-column prop="type" label="事项类型" width="100" />
		<el-table-column prop="content" label="内容" width="775" />
		<el-table-column prop="initiator" label="发起人" width="80" />
		<el-table-column prop="initiationTime" label="发起时间" width="160" />
		<el-table-column label="操作" width="120">
			<template #default="scope">
				<!-- 				<el-button @click="complete(scope.$index, scope.row)" v-if="scope.row.status=='False'">完成</el-button>
				<el-button @click="undo(scope.$index, scope.row)" v-if="scope.row.status=='True'">撤销</el-button> -->

				<el-select v-model="scope.row.status" class="m-2"
					@change="changeOption(scope.$index, scope.row)">
					<el-option v-for="item in status" :key="item" :label="item" :value="item"/>
				</el-select>
			</template>
		</el-table-column>
	</el-table>
</template>

<script>
	//导入自定义js文件
	import {
		sortKeyAsc,
		sortKeyDesc,
		trans
	} from '../../assets/js/tool.js'
	//导入消息提示
	import {
		ElMessage
	} from 'element-plus'
	export default {
		name: "TodoItems",
		data() {
			return {
				loading: false,
				formData: [],
				status: ['未完成', '完成'],
				aa: ''
			}
		},
		methods: {
			delLine({
				row,
				column,
				rowIndex,
				columnIndex
			}) {
				if (row.status == '完成' && columnIndex != '4') {
					return 'del_line'
				}
			},
			backColor({
				row,
				rowIndex
			}) {
				if (row.priority == '1') {
					return 'importantBackColor'
				}
			},
			//完成按钮事件
			complete(index, row) {
				let id = this.formData[index].id
				if (this.updateItemStatus(id, 1)) {
					this.formData[index].status = 'True'
				}
			},
			//撤销按钮事件
			undo(index, row) {
				let id = this.formData[index].id
				if (this.updateItemStatus(id, 0)) {
					this.formData[index].status = 'False'
				}
			},
			//按优先级排序数据
			sortData() {
				sortKeyDesc(this.formData, 'priority')
			},
			//把事项内的false和true，转成 完成 和 未完成
			transStatus() {
				trans(this.formData)
			},
			async getToDoItems(userName) {
				this.loading = true
				const {
					data: res
				} = await this.$http.get('api/getToDoItems.ashx', {
					params: {
						userName: userName
					},
				})

				if (res.code !== 0) return console.log("获取待办事项失败")

				this.formData = res.data
				this.loading = false
				this.transStatus()
				this.sortData()
			},

			async updateItemStatus(id, status) {
				console.log(id + "   " + status)
				const {
					data: res
				} = await this.$http.post('api/updateItemStatus.ashx', {
					params: {
						id: id,
						status: status
					},
				})
				if (res.code !== 0) return false
				ElMessage({
					message: '待办事项已进行更新',
					type: 'success',
				})
				return true
			},
			fail(msg) {
				ElMessage({
					message: msg,
					type: 'error',
				})
			},
			changeOption(index, row) {
				let id = this.formData[index].id
				if (row.status === "完成") {
					this.updateItemStatus(id, 1)
				} else if (row.status === "未完成") {
					this.updateItemStatus(id, 0)
				}
			}
		},
		mounted() {
			this.getToDoItems(localStorage.getItem('user'))
		},
		computed: {

		}
	}
</script>

<style lang="less">
	/* 删除线 */
	.del_line::after {
		content: no-open-quote;
		position: absolute;
		top: 51%;
		left: 0;
		width: 100%;
		border-bottom: 1px solid red;
	}

	.importantBackColor {
		color: red;
	}
</style>
