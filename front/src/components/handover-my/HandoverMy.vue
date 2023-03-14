<template>
	<div>
	<el-table :data="itemData.data" v-loading="loading" style="margin: auto;" align="center" border 
		width="auto" element-loading-background="rgba(255, 255, 255, 0)" @sort-change='sortTableFun'>
		<el-table-column prop="type" label="事项类型" width="100" />
		<el-table-column prop="content" label="内容" width="730" />
		<el-table-column prop="recipient" label="处理人" width="80" />
		<el-table-column prop="priority" label="优先级" width="100" sortable='custom' >
			<template #default="scope">
			<el-tag :type="scope.row.priority==='1'?'danger':'success'">
			{{scope.row.priority==1?"重要":"普通"}}
			</el-tag>
			</template>
		</el-table-column>
		<el-table-column prop="status" label="当前状态" width="110" sortable='custom'>
			<template #default="scope">
				<el-tag disable-transitions>{{scope.row.status === 'True' ? '完成' : '未完成'}}
				</el-tag>
			</template>
		</el-table-column>
		<el-table-column prop="initiationTime" label="发起时间" width="155" sortable='custom' />
	</el-table>


	<div style="display: flex;justify-content: center;">


		<!-- 分页区域 -->
		<el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange"
			v-model:current-page="queryInfo.pagenum" v-model:page-size="queryInfo.pagesize" :page-sizes="[20,30,40,50]"
			layout="total, sizes, prev, pager, next, jumper" :total="itemData.count" />
	</div>
	</div>
</template>

<script>
	export default {
		name: "HandoverMy",
		data() {
			return {
				itemData: [],
				//分页参数
				queryInfo: {
					query: '',
					pagenum: 1,
					pagesize: 20
				},
				loading: false,
				initiator: '',
				orderByItem:'',
				collation:'',
			}
		},
		created() {
			this.initiator = localStorage.getItem('user')
			this.getMyItems()
		},
		methods: {
			async getMyItems() {
				this.loading = true
				const {
					data: res
				} = await this.$http.get("/api/getInitiatedItems.ashx", {
					params: {
						page: this.queryInfo.pagenum,
						limit: this.queryInfo.pagesize,
						initiator: this.initiator,
						orderByItem:this.orderByItem,
						collation:this.collation,
					},
				})

				if (res.code == 0) {
					this.itemData = res
					this.loading = false
				}
			},
			//监听page size改变的事件
			handleSizeChange(newSize) {
				this.queryInfo.pagesize = newSize
				if (this.itemData.count > newSize - 10) {
					//刷新数据
					this.getMyItems()
				}
			},
			//监听页码值改变的事件
			handleCurrentChange(newPage) {
				this.queryInfo.pagenum = newPage
				//刷新数据
				this.getMyItems()
			},
			 sortTableFun(column){//用户点击这一列的上下排序按钮时，触发的函数
			          //this.orderByItem = column.prop; //该方法获取到当前列绑定的prop字段名赋值给一个变量，之后这个变量做为入参传给后端
			            if (column.prop) { //该列有绑定prop字段走这个分支
						    if(column.prop=='initiationTime'){
								this.orderByItem='HANDOVER_CREATETIME'
							}
							else if(column.prop=='priority')
							{
								this.orderByItem='HANDOVER_PRIORITY'
							}
							else if(column.prop=='status')
							{
								this.orderByItem='HANDOVER_STATUS'
							}
			              if (column.order == 'ascending') {//当用户点击的是升序按钮，即ascending时
			                  this.collation = 'asc'; //将order这个变量赋值为后端接口文档定义的升序的字段名，之后作为入参传给后端
			              } else if (column.order == 'descending') {
			              //当用户点击的是升序按钮，即descending时
			                  this.collation = 'desc';//将order这个变量赋值为后端接口文档定义的降序的字段名，之后作为入参传给后端
			              }
			                  this.getMyItems()//且发起后端请求的接口
			            }
			 },
		}

	}
</script>

<style lang="less">
</style>
