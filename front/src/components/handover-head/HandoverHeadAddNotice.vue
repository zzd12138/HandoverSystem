<template>
	<el-dialog v-model="dialogFormVisible2" title="增加通告" @close="closeDialog" :width="500" v-loading="loading" element-loading-background="rgba(255, 255, 255, 0)">
		<el-form :label-position="labelPosition" :rules="rules" :model="notice" ref="notice">
			<el-form-item label="公告主题:" :label-width="formLabelWidth" prop="title">
				<el-input v-model.number="notice.title" autocomplete="off" />
			</el-form-item>
			<el-form-item label="公告内容:" :label-width="formLabelWidth" prop="content">
				<el-input v-model="notice.content" autocomplete="off" type="textarea" :rows="5" :maxlength="200" />
			</el-form-item>
		</el-form>
		<el-form style="text-align: center;">
			<el-button type="primary" @click="submit('notice')">提交</el-button>
		</el-form>
	</el-dialog>
</template>

<script>
	//数据转化的组件
	import qs from  'qs'
	//导入消息提示
	import {
		ElMessage
	} from 'element-plus'
	export default {
		name: "HandoverHeadAddNotice",
		props: {
			dialogFormVisible: {
				type: Boolean,
				require: true,
			}
		},
		data() {
			return {
				notice: [{
					content: "",
					title: "",
					initiator:''
				}],
				dialogFormVisible2: false,
				labelPosition: "top",
				loading:false,
				//定时器
				timer: "",
				//表单校验
				rules: {
					content: [{
						required: true,
						message: '请输入',
						trigger: 'blur'
					}],
					title: [{
							required: true,
							message: '请输入',
							trigger: 'blur'
						},
					]
				},
			}
		},
		methods: {
			/**
			 * 关闭弹出框事件，触发父级组件的closeChildDialog方法
			 */
			closeDialog() {
				this.$emit('closeChildDialog')
				//清空表单数据
				this.notice = []
			},
			submit(notice)
			{
				this.$refs[notice].validate(valid => {
					if (valid) {
						this.addNotice()
						this.loading=true
						//设置loading 9秒后关闭
				        this.timer=setTimeout(this.loadingEnd,9000)
					} else {
						console.log('表单验证未通过')
					}
				})
			},
			async addNotice(){
				this.notice.initiator=this.$parent.user
				//把表单数据转换一遍
				let data = qs.stringify(this.notice)
				
				const {
					data: res
				} = await this.$http.post('api/addNotice.ashx', data)
				
				if (res.code !== 0) return this.fail(res.msg)
				this.loading=false
				this.success()
				//发送 提交成功后回调函数
				this.$emit('addSuccess')
			    this.notice={}
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
			//关闭loading
			loadingEnd(){
				this.loading=false
			}
		},
		watch: {
			dialogFormVisible(newVal, oldVal) {
				this.dialogFormVisible2 = newVal
			},
		}
	}
</script>

<style lang="less" scoped>
</style>
