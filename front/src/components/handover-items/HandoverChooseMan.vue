<template>
	<el-dialog v-model="dialogFormVisible2" title="选择交接人" @close="closeDialog" :width="500"
		style="padding-bottom: 20px;" v-loading="loading" element-loading-background="rgba(255, 255, 255, 0)">
		<div style="text-align: center;">
			<el-radio-group v-model="man" size="large">
				<el-radio-button v-for="man in mans" :key="man" :label="man"></el-radio-button>
			</el-radio-group>
			<p>
			<el-switch
			  v-model="isTransmittedUnDoItems"
			  active-text="自动交接未完成事项"
			  class="switch">
			</el-switch>
			</p>
		</div>
		<div style="margin-top: 20px;margin-top: 50px;">
			<el-button type="primary" style="float: right;" @click="submitMan">提交</el-button>
			<el-button style="float: right;margin-right: 20px;" @click="cancelSubmit">取消</el-button>
		</div>
	</el-dialog>
</template>

<script>
	//数据转化的组件
	import qs from 'qs'
	import {
		ElMessage
	} from 'element-plus'

	export default {
		name: "HandoverChooseMan",
		props: {
			dialogFormVisible: {
				type: Boolean,
				require: true,
				default: false
			},
			mans: {
				type: Array,
				require: true,
				default: ""
			},
		},

		data() {
			return {
				loading:false,
				dialogFormVisible2: false,
				man: this.mans[0],
				//获取父组件表单信息
				formData:this.$parent.$parent.formData,
				//是否交接未完成事项开关
				isTransmittedUnDoItems:true,
				//定时器
				timer: "",
			}
		},
		created() {
			//
			this.timer=setTimeout(this.setMan,2000)
		},
		methods: {
			cancelSubmit() {
				this.dialogFormVisible2 = false
			},
			async submitMan() {

				//把表单数据转换一遍
				//let data = qs.stringify(this.formData)
				// 1. 通过组件实例 this 访问到全局挂载的 $http 属性，并发起Ajax 数据请求
				this.loading=true
				//设置loading 9秒后关闭
				this.timer=setTimeout(this.loadingEnd,9000)	
				//接收人赋值
				this.formData.recipient=this.man
				//自动交接未完成事项开关赋值
				this.formData.isTransmittedUnDoItems=this.isTransmittedUnDoItems
						
				const {
					data: res
				} = await this.$http.post('api/addHandoverItems.ashx', this.formData)
				
				if(res.code!==0) return this.fail(res.msg)
				//提交数据
				ElMessage({
					message: '提交成功',
					type: 'success',
				})
				this.loading=false
				//提交成功后的回调函数
				this.$emit('submitSuccess')
				//关闭窗口
				this.dialogFormVisible2 = false
			},
			//关闭loading
			loadingEnd(){
				this.loading=false
			},
			fail(msg) {
				ElMessage({
					message: msg,
					type: 'error',
				})
			},
			setMan(){
				this.man=this.mans[0];
			}
		},
		watch: {
			dialogFormVisible(newVal, oldVal) {
				this.dialogFormVisible2 = newVal
			},
			dialogFormVisible2(newVal, oldVal) {
				if (newVal == false) this.$emit('closeChildDialog')
			}
		}
	}
</script>

<style lang="less" scoped>
.switch{
	text-align: center;
	float: right;
}
</style>
