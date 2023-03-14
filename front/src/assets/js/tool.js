//正序排序
		function sortKeyAsc(array, key) {
			return array.sort(function(a, b) {
				var x = a[key];
				var y = b[key];
				return ((x < y) ? -1 : (x > y) ? 1 : 0)
			})
		}
		//倒序
		function sortKeyDesc(array, key) {
			return array.sort(function(a, b) {
				var x = a[key];
				var y = b[key];
				return ((x > y) ? -1 : (x < y) ? 1 : 0)
			})
		}
		
		function trans(array){
			let arrNew;
			for(let index in array)
			{
				if(array[index].status==="True"){
				array[index].status="完成"
				}
				else{
					array[index].status="未完成"
				}
			}
		}
		
			export{sortKeyAsc,sortKeyDesc,trans}
