<template>
	<div class="main">
		<div v-for="(importantInfo,index) in importantInfo" :key="importantInfo.key">
			<el-row :gutter="1">
				<el-col :lg="2" :xs="24" style="height: 55px;margin-top: 12px;">
					{{label}}{{index+1}}
				</el-col>

				<el-col :lg="16" :xs="24" style="height: 55px;">
					<span>内容：</span>
					<el-input type="textarea" placeholder="请输入内容" @change="transmitImportantInfo" style="width: 90%;margin-top: 0px;height: 33px;"
						v-model="importantInfo.content" />
				</el-col>

				<el-col :lg="4" :xs="12" style="margin-top: 12px;height: 55px;">优先级：
					<el-select v-model="importantInfo.priority" class="m-2" style="width: 80px;" @change="transmitImportantInfo">
						<el-option v-for="item in priorityOption" :key="item.value" :label="item.label"
							:value="item.value" />
					</el-select>
				</el-col>
				<el-col :lg="2" :xs="12" style="margin-top: 12px;height: 55px;">
					<el-icon :size="30" color="PaleGreen" @click="addNewImportantInfo()" v-if="index==0" style="cursor: pointer;"><CirclePlus/></el-icon>
					<el-button type="danger" v-if="index!=0" @click="deleteHandoverItemList(index)">删除</el-button>
				</el-col>
			</el-row>
		</div>
	</div>
</template>

<script>
	//导入图标
	import {
		CirclePlus
	} from '@element-plus/icons-vue'
	export default {
		name: "HandoverItemList",
		emits:['getImportantInfo'],
		props: {
			label: {
				type: String,
				require: true,
				default: "重点警情"
			},
		},
		data() {
			return {
				priority: "",
				content: "",
					importantInfo: [{
							content: "",
							priority: "0"},
					],
				priorityOption: [{
						value: "0",
						label: "普通"
					},
					{
						value: "1",
						label: "重要"
					},
				],
			}
		},
		methods:{
			//新增事项
			addNewImportantInfo(){
				this.importantInfo.push({
					content:"",
					priority:"0"
				})
			},
			//删除事项
			deleteHandoverItemList(index){
				this.importantInfo.splice(index,1)
				this.transmitImportantInfo()
			},
			//向父组件传输数据
			transmitImportantInfo(){
				this.$emit('getImportantInfo',this.importantInfo)
			}
		},
		components:{
			CirclePlus,
		}
	}
</script>

<style lang="less" scoped>
	.main {
		margin: 0 auto;
		justify-content: center;
		align-items: center;
		font-size: 15px;
		padding: 0px;
		height: 55px;
	}

</style>
