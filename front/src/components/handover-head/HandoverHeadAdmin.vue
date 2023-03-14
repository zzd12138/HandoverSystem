<template>
	<el-dialog v-model="dialogFormVisible2" title="管理员界面" @close="closeDialog" :width="700">
		<el-menu :default-active="activeIndex" class="el-menu-demo" mode="horizontal" @select="handleSelect">
			<el-menu-item index="1" @click="userShow=true,regShow=false">用户管理</el-menu-item>
			<el-menu-item index="2" @click="regShow=true,userShow=false">新用户注册</el-menu-item>
		</el-menu>
		
		
		<!-- 用户管理界面 -->
		<el-collapse-transition>
		<el-table :data="users" style="margin: auto;" align="center" border height="352" width="auto"
			v-loading="loading" element-loading-background="rgba(255, 255, 255, 0)" v-show="userShow">
			<el-table-column prop="policeNumber" label="警号" width="200" />
			<el-table-column prop="userName" label="姓名" width="200" />
			<el-table-column label="操作" width="260">
				<template #default="scope">
					<el-button @click="modifyPwd(scope.row)">修改密码</el-button>
				    <el-button @click="confirmDelete(scope.row)" type="danger">删除</el-button>
				</template>
			</el-table-column>
		</el-table>
	</el-collapse-transition>
		
		<!-- 修改密码界面 -->
		<handover-head-modify-pwd :policeNumber="policeNumber" :dialogFormVisible="modifyPwdDialogVisible" @closeChildDialog="closePwdDialog" @success="modifyPwdSuccess">
		</handover-head-modify-pwd>
		
        <!-- 用户注册界面 -->
		<el-collapse-transition>
		<el-form :label-position="labelPosition" :rules="rules" :model="regInfo" ref="regInfo" v-show="regShow">
			<el-form-item label="姓名:" prop="userName">
				<el-input v-model="regInfo.userName" :prefix-icon="User" autocomplete="off" />
			</el-form-item>
			<el-form-item label="警号:" prop="policeNumber">
				<el-input v-model="regInfo.policeNumber" autocomplete="off" :prefix-icon="Postcard" />
			</el-form-item>
			<el-form-item label="密码:" prop="password">
				<el-input v-model="regInfo.password" type="password" autocomplete="off" :prefix-icon="Lock" />
			</el-form-item>
			<el-form-item label="再次输入密码:" prop="passwordCompare">
				<el-input v-model="regInfo.passwordCompare" type="password" autocomplete="off" :prefix-icon="Lock" />
			</el-form-item>
			<div style="text-align: center;">
			<el-button type="primary" @click="reg('regInfo')">注册</el-button>
			</div>
		</el-form>
	</el-collapse-transition>
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
	import HandoverHeadModifyPwd from './HandoverHeadModifyPwd.vue'
	export default {
		name: "HandoverHeadAdmin",
		props: {
			dialogFormVisible: {
				type: Boolean,
				require: true,
				default: true
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
				dialogFormVisible2: false,
				modifyPwdDialogVisible: false,
				activeIndex: '1',
				regShow: false,
				userShow:true,
				policeNumber:'',

				regInfo: {
					userName: '',
					password: '',
					passwordCompare: '',
					policeNumber: ''
				},
				users:[],
				labelPosition: "top",
				//表单校验
				rules: {
					userName: [{
							required: true,
							message: '请输入姓名',
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
					policeNumber: [{
						required: true,
						message: '请输入警号',
						trigger: 'blur'
					},
					{
						min: 6,
						max: 8,
						message: '长度在 6 到 8 个字符',
						trigger: 'blur'
					},],
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
						policeNumber: this.regInfo.policeNumber
					},
				})

				// 2. 判断请求是否成功
				if (res.code !== 0) return this.regFail(res.msg)

				ElMessage({
					message: '注册成功',
					type: 'success',
				})

				this.regInfo = {}
				this.getAllUsers()

			},
			regFail(msg) {
				ElMessage({
					message: msg,
					type: 'error',
				})
			},
			//获取所有用户信息
			async getAllUsers() {
				const {
					data: res
				} = await this.$http.get('api/getUsers.ashx')
				if (res.code !== 0) return console.log("获取用户失败")
				this.users=res.data
				
			},
			//修改密码按钮事件
			modifyPwd(row){
				this.policeNumber=row.policeNumber
				this.modifyPwdDialogVisible=true
			},
			//关闭修改密码页面回调函数
			closePwdDialog(){
				this.modifyPwdDialogVisible=false
			},
			//修改密码成功回调函数
			modifyPwdSuccess(){
				this.modifyPwdDialogVisible=false
			},
			//删除按钮事件
			 confirmDelete(row) {
			        this.$confirm('确认删除该用户吗?', '提示', {
			          confirmButtonText: '确定',
			          cancelButtonText: '取消',
			          type: 'warning'
			        }).then(() => {
						this.deleteUser(row.policeNumber);
			        }).catch(() => {
						
			        });
			      },
			async deleteUser(policeNumber){
						  const{data:res}=await this.$http.get('/api/deleteUser.ashx',{
							  params: {
							  	policeNumber: policeNumber,
							  },
						  })
						  // 2. 判断请求是否成功
						  if (res.code !== 0) return this.regFail(res.msg)
						  			
						  ElMessage({
						  	message: '删除成功',
						  	type: 'success',
						  })
						  this.getAllUsers()
					  }
		},
		mounted() {
			this.getAllUsers()
		},
		watch: {
			dialogFormVisible(newVal, oldVal) {
				this.dialogFormVisible2 = newVal
			},
		},
		components:{
			HandoverHeadModifyPwd
		}
	}
</script>

<style lang="less" scoped>
</style>
