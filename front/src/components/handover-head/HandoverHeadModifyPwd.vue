<template>
	<el-dialog v-model="dialogFormVisible2" title="修改密码" @close="closeDialog" :width="400">
		<el-form :label-position="labelPosition" :rules="rules" :model="regInfo" ref="regInfo">
			<el-form-item label="密码:" prop="password">
				<el-input v-model="regInfo.password" type="password" autocomplete="off" :prefix-icon="Lock" />
			</el-form-item>
			<el-form-item label="再次输入密码:" prop="passwordCompare">
				<el-input v-model="regInfo.passwordCompare" type="password" autocomplete="off" :prefix-icon="Lock" />
			</el-form-item>
		</el-form>
		<el-form style="text-align: center;">
			<el-button type="primary" @click="modifyPwd('regInfo')">修改</el-button>
		</el-form>
	</el-dialog>
</template>

<script>
	//导入图标
	import {
		Lock,
	} from '@element-plus/icons-vue'
	//导入消息提示
	import {
		ElMessage
	} from 'element-plus'
	export default{
		name:"HandoverHeadModifyPwd",
		props: {
			dialogFormVisible: {
				type: Boolean,
				require: true,
				default: false
			},
			policeNumber:{
				type:String,
				require:false
			},
		},
		data() {
			var validatePass2 = (rule, value, callback) => {
				if (value === '') {
					callback(new Error('请再次输入密码'));
				} else if (value !== this.regInfo.password) {
					callback(new Error('两次输入密码不一致!'));
				} else {
					callback();
				}
			};
			return{
				regInfo: {
					password: '',
					passwordCompare: '',
				},
				dialogFormVisible2: false,
				labelPosition: "top",
				//表单校验
				rules: {
					password: [{
							required: true,
							message: '请输入密码',
							trigger: 'blur'
						},
						{
							min: 6,
							max: 16,
							message: '长度在 6 到 16 个字符',
							trigger: 'blur'
						},
					],
					passwordCompare: [{
							required: true,
							message: '请再次输入密码',
							trigger: 'blur'
						},
						{
							min: 6,
							max: 16,
							message: '长度在 6 到 16 个字符',
							trigger: 'blur'
						},
						{
							validator: validatePass2,
							trigger: 'blur',
							required: true
						}
					],
				}
			}
		},
		methods:{
			modifyPwd(regInfo) {
				this.$refs[regInfo].validate(valid => {
					if (valid) {
						//判断是否用管理员账号修改密码
						let policeNumber
						if(this.policeNumber==null || this.policeNumber.length==0){
							policeNumber=localStorage.getItem('policeNumber')
						}
						else
						{
							policeNumber=this.policeNumber
						}
						this.ToModify(policeNumber)
					} else {
						console.log('表单验证未通过')
					}
				})
			},
			async ToModify(policeNumber) {
				//1. 通过组件实例 this 访问到全局挂载的 $http 属性，并发起Ajax 数据请求
				const {
					data: res
				} = await this.$http.get('api/modifyPwd.ashx', {
					params: {
						policeNumber: policeNumber,
						password: this.regInfo.password,
					},
				})
			
				// 2. 判断请求是否成功
				if (res.code !== 0) return this.regFail(res.msg)
			
				ElMessage({
					message: '修改成功',
					type: 'success',
				})
				
				this.$emit('success');
			
			},
			regFail(msg) {
				ElMessage({
					message: msg,
					type: 'error',
				})
			},
			/**
			 * 关闭弹出框事件，触发父级组件的closeChildDialog方法
			 */
			closeDialog() {
				this.$emit('closeChildDialog')
				//清空表单数据
				this.regInfo = {}
			},
		},
		watch: {
			dialogFormVisible(newVal, oldVal) {
				this.dialogFormVisible2 = newVal
			},
		},
		//进行图标注册
		setup() {
			return {
				Lock
			}
		},
	}
</script>

<style lang="less" scoped>
</style>