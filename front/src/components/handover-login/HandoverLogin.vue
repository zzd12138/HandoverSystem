<template>
	<div class="login-main">
		<el-form :label-position="right" label-width="100px" :model="userInfo" :rules="rules" ref="userInfo"
			class="login-form" v-loading="loading" element-loading-background="rgba(255, 255, 255, 0)">
			<el-form-item>
				<p class="login-wel">值班交接</p>
			</el-form-item>
			<el-form-item label="警号:" prop="policeNumber">
				<el-input v-model="userInfo.policeNumber" :prefix-icon="User" autocomplete="off" />
			</el-form-item>
			<el-form-item label="密码:" prop="password">
				<el-input @keyup.enter="onsubmit('userInfo')" v-model="userInfo.password" type="password"
					autocomplete="off" :prefix-icon="Lock" />
			</el-form-item>

			<el-form-item class="login-button">
				<el-button type="primary" @click="onsubmit('userInfo')">登陆</el-button>
			</el-form-item>
		</el-form>
		<div class="copyright">©2023 嘉兴市公安局秀洲区分局 科通</div>
	</div>
	<handover-reg :dialogFormVisible="dialogFormVisible" @closeChildDialog="this.dialogFormVisible=false">
	</handover-reg>>
</template>

<script>
	//导入md5加密
	import md5 from 'js-md5'
	//导入图标
	import {
		User,
		Lock
	} from '@element-plus/icons-vue'
	//导入消息提示
	import {
		ElMessage
	} from 'element-plus'
	import HandoverReg from './HandoverReg.vue'

	export default {
		name: 'HandoverLogin',
		data() {
			return {
				userInfo: {
					policeNumber: '',
					password: ''
				},
				dialogFormVisible: false,

				//表单校验
				rules: {
					policeNumber: [{
						required: true,
						message: '请输入',
						trigger: 'blur'
					}],
					password: [{
						required: true,
						message: '请输入',
						trigger: 'blur'
					}]
				},
				//登陆加载状态
				loading: '',
				//定时器
				timer: "",
			}
		},
		methods: {
			onsubmit(formName) {
				this.$refs[formName].validate(valid => {
					if (valid) {
						this.login()
						this.loading = true
						//设置loading 9秒后关闭
						this.timer = setTimeout(this.loadingEnd, 9000)
					} else {
						console.log('表单验证未通过')
					}
				})
			},
			async login() {
				//1. 通过组件实例 this 访问到全局挂载的 $http 属性，并发起Ajax 数据请求
				const {
					data: res
				} = await this.$http.get('api/login.ashx', {
					params: {
						policeNumber: this.userInfo.policeNumber,
						password: md5(this.userInfo.password),
					},
				})

				// 2. 判断请求是否成功
				if (res.code !== 0) return this.loginFail(res.msg)

				this.loading = false
				ElMessage({
					message: '登陆成功',
					type: 'success',
				})
				//跳转页面
				this.$router.push('/index')
				// 存储 token
				localStorage.setItem('user', res.data[0].userName)
				localStorage.setItem('policeNumber', res.data[0].policeNumber)
			},
			loginFail(msg) {
				ElMessage({
					message: msg,
					type: 'error',
				})
				this.loading = false
			},
			//关闭loading
			loadingEnd() {
				this.loading = false
			}
		},
		components: {
			HandoverReg
		},
		//进行图标注册
		setup() {
			return {
				User,
				Lock
			}
		},

	}
</script>

<style lang="less" scoped>
	.login-form {
		border: 1px;
		padding-right: 50px;
		padding-top: 20px;
		position: absolute;
		top: 0;
		bottom: 0;
		left: 0;
		right: 0;
		margin: auto;
		width: 300px;
		height: 200px;
		background-color: rgba(255, 255, 255, 0.8);
		border-radius: 5px;
	}

	.login-main {
		position: absolute;
		//background-color: beige;
		background-image: url("/img/bg1.gif");
		background-size: 100% 100%;
		width: 100%;
		height: 100%;
		margin: 0;
		padding: 0;
	}

	.login-wel {
		font-size: 20px;
		margin-left: 35px;
		margin-bottom: 0px;
		margin-top: 0px;
	}

	.login-button {
		margin-left: 50px;
	}

	.slide-verify-back {
		position: absolute;
		width: 100%;
		height: 100%;
		top: 0;
		bottom: 0;
		left: 0;
		right: 0;
		margin: auto;
		z-index: 999;
		background-color: rgba(190, 190, 190, 0.7);
	}

	.copyright {
		position: absolute;
		bottom: 30px;
		left: 0;
		right: 0;
		text-align: center;
	}
</style>
