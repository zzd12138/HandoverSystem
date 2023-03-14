<template>
	<el-dialog v-model="dialogFormVisible2" title="注册" @close="closeDialog" :width="400">
		<el-form :label-position="labelPosition" :rules="rules" :model="regInfo" ref="regInfo">
			<el-form-item label="姓名:" prop="userName">
				<el-input v-model="regInfo.userName" :prefix-icon="User" autocomplete="off" />
			</el-form-item>
			<el-form-item label="密码:" prop="password">
				<el-input v-model="regInfo.password" type="password" autocomplete="off" :prefix-icon="Lock" />
			</el-form-item>
			<el-form-item label="再次输入密码:" prop="passwordCompare">
				<el-input v-model="regInfo.passwordCompare" type="password" autocomplete="off" :prefix-icon="Lock" />
			</el-form-item>
			<el-form-item label="邀请码:" prop="invitationCode">
				<el-input v-model="regInfo.invitationCode" autocomplete="off" :prefix-icon="Postcard" />
			</el-form-item>
		</el-form>
		<el-form style="text-align: center;">
			<el-button type="primary" @click="reg('regInfo')">注册</el-button>
		</el-form>
	</el-dialog>
</template>

<script>
	//导入图标
	import {
		User,
		Lock,
		Postcard
	} from '@element-plus/icons-vue'
	//导入消息提示
	import {
		ElMessage
	} from 'element-plus'
	export default {
		name: "HandoverReg",
		props: {
			dialogFormVisible: {
				type: Boolean,
				require: true,
				default: false
			}
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
			return {
				regInfo: {
					userName: '',
					password: '',
					passwordCompare: '',
					invitationCode: ''
				},
				dialogFormVisible2: false,
				labelPosition: "top",
				//表单校验
				rules: {
					userName: [{
							required: true,
							message: '请输入',
							trigger: 'blur'
						},
						{
							min: 2,
							max: 6,
							message: '长度在 2 到 6 个字符',
							trigger: 'blur'
						},
						{
							validator: function(rule, value, callback) {

								var specialKey =
									new RegExp(
										"[ \\[ \\] \\^ \\-_*×――(^)$%~!＠@＃#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;/\'\"{ }（）‘’“”-]"
										);
								if (specialKey.test(value)) {
									callback(new Error("用户名不能包含特殊字符"));
								} else {
									//校验通过
									callback();
								}
							},
							trigger: 'blur'
						},
					],
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
					invitationCode: [{
						required: true,
						message: '请输入',
						trigger: 'blur'
					}],
				}
			}
		},
		methods: {
			reg(regInfo) {
				this.$refs[regInfo].validate(valid => {
					if (valid) {
						this.ToReg()
					} else {
						console.log('表单验证未通过')
					}
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

			async ToReg() {

				//1. 通过组件实例 this 访问到全局挂载的 $http 属性，并发起Ajax 数据请求
				const {
					data: res
				} = await this.$http.get('api/reg.ashx', {
					params: {
						userName: this.regInfo.userName,
						password: this.regInfo.password,
						invitationCode: this.regInfo.invitationCode
					},
				})

				// 2. 判断请求是否成功
				if (res.code !== 0) return this.regFail(res.msg)

				ElMessage({
					message: '注册成功',
					type: 'success',
				})

				this.regInfo = {}

			},
			regFail(msg) {
				ElMessage({
					message: msg,
					type: 'error',
				})
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
				User,
				Lock,
				Postcard
			}
		},
	}
</script>

<style lang="less" scoped>

</style>
