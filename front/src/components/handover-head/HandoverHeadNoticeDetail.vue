<template>
	<el-dialog v-model="detailDialogVisible2" title="通告详情" @close="closeDialog()" @open="openDialog" width="700">
		<el-form :label-position="right" label-width="70px" :model="formLabelAlign"
			style="max-width: 560px;margin: 0 auto;">
			<el-form-item label="主题:" :label-width="formLabelWidth">
				<el-input v-model="data.title" :readonly="false" autocomplete="off" />
			</el-form-item>
			<el-form-item label="内容:" :label-width="formLabelWidth">
				<el-input v-model="data.content" :readonly="false" autocomplete="off" type="textarea" :rows="10" />
			</el-form-item>
			<el-form-item label="发起人:" :label-width="formLabelWidth">
				<el-input v-model="details.initiator" :readonly="true" autocomplete="off" />
			</el-form-item>
			<el-form-item>
				<el-button type="danger" :size="large" style="position: absolute;right: 0%;margin-top: 10px;"
					@click="confirmDelete">删除通告</el-button>
				<el-button type="success" :size="large" style="position: absolute;right:20%;margin-top: 10px;"
					@click="confirmModify">提交修改</el-button>	
			</el-form-item>
		</el-form>
	</el-dialog>
</template>

<script>
	import {
		ElMessage
	} from 'element-plus'
		import qs from 'qs'
	export default {
		name: "HandoverHeadNoticeDetail",
		props: {
			details: {
				type: Object,
				required: true,
			},
			detailDialogVisible: {
				type: Boolean,
				required: true,
			},
		},
		data() {
			return {
				detailDialogVisible2: false,
				data:{
					id:'',
					title:'',
					content:''
				},
			}
		},
		methods: {
			/**
			 * 关闭弹出框事件，触发父级组件的closeChildDialog方法
			 */
			closeDialog() {
				this.$emit('closeDetailDialog',this.details.id)
			},
			openDialog() {
				this.$emit('openDetailDialog')
			},			
			//删除按钮事件
			 confirmDelete() {
			        this.$confirm('确认删除该通告吗?', '提示', {
			          confirmButtonText: '确定',
			          cancelButtonText: '取消',
			          type: 'warning'
			        }).then(() => {
						this.deleteNotice();
			        }).catch(() => {
						
			        });
			      },	
			async deleteNotice() {
				const {
					data: res
				} = await this.$http.get('/api/deleteNotice.ashx',{
					params: {
						id: this.details.id,
						}
					})
				if (res.code !== 0) return this.fail(res.msg)

				//提交成功后的回调函数
				this.$emit('deleteSuccess')
				this.success('删除成功')
			},
			
			
			//修改按钮事件
			 confirmModify() {
			        this.$confirm('确认修改该通告吗?', '提示', {
			          confirmButtonText: '确定',
			          cancelButtonText: '取消',
			          type: 'warning'
			        }).then(() => {
						this.ModifyNotice();
			        }).catch(() => {
						
			        });
			      },			  
				async ModifyNotice() {
					//把表单数据转换一遍
					let data=qs.stringify(this.data)
					const {
						data: res
					} = await this.$http.post('/api/ModifyNotice.ashx',data)
					if (res.code !== 0) return this.fail(res.msg)
				
					//提交成功后的回调函数
					this.$emit('deleteSuccess')
					this.success('修改成功')
				},  
				  
				  
			fail(msg) {
				ElMessage({
					message: msg,
					type: 'error',
				})
			},
			success(msg='提交成功') {
				ElMessage({
					message: msg,
					type: 'success',
				})
			},
		},
		watch: {
			detailDialogVisible(newVal, oldVal) {
				this.detailDialogVisible2 = newVal
				this.data.id=this.details.id
				this.data.title=this.details.title
				this.data.content=this.details.content
			}
		}
	}
</script>

<style lang="less">
	.previewDialog.el-dialog {
		.el-dialog__header {
			display: none;
		}

		.dj-dialog-content {
			padding: 0;
			overflow: unset;
		}
	}
</style>
