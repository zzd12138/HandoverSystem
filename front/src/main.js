import {
	createApp
} from 'vue'
import App from './App.vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import zhCn from 'element-plus/dist/locale/zh-cn.mjs'
import router from './router.js'
import './style.css'
// 导入 axios
import axios from 'axios'
//导入消息提示
import {
	ElMessage
} from 'element-plus'

const app = createApp(App)
//挂载路由对象
app.use(router)
//挂载element
app.use(ElementPlus, {
	locale: zhCn,
})



// 将 axios 挂载为全局的 $http 自定义属性
app.config.globalProperties.$http = axios
axios.defaults.timeout = 10000

// 添加响应拦截器
axios.interceptors.response.use(function(response) {
	// 2xx 范围内的状态码都会触发该函数。
	// 对响应数据做点什么
	//console.log(response)
	return response;
}, function(error) {
	// 超出 2xx 范围的状态码都会触发该函数。
	// 对响应错误做点什么
	if (error.code === 'ECONNABORTED' || error.code === 'ECONNABORTED') {
		ElMessage({
			message: '网络连接超时，请重试',
			type: 'error',
		})

	}
	return Promise.reject(error);
});





app.mount('#app')
