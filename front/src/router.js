// 1. 按需导入对应的函数
import {
	createRouter,
	createWebHashHistory
} from 'vue-router'

import Index from './components/handover-index/HandoverIndex.vue'
import HandoverLogin from './components/handover-login/HandoverLogin.vue'
import HandoverItems from "./components/handover-items/HandoverItems.vue"
import TodoItems from "./components/todo-items/TodoItems.vue"
import HandoverMy from "./components/handover-my/HandoverMy.vue"

// 2. 创建路由对象
const router = createRouter({
	history: createWebHashHistory(),
	routes: [
		//路由重定向
		{
			path: '/',
			redirect: '/index'
		},
		{
			path: '/login',
			component: HandoverLogin	
		},
		{
			path: '/index',
			component: Index,
			redirect:'/index/HandoverItems',
			children:[
				{path:'HandoverItems',component:HandoverItems},
				{path:'TodoItems',component:TodoItems},
				{path:'HandoverMy',component:HandoverMy},
			]
		},
	],
})

//全局路由导航守卫
router.beforeEach((to, from, next) => {
	//访问的是登录页，直接放行
	if (to.path === '/login') {
		return next()
	}
	//获取token
	const user = localStorage.getItem('user')
	//没有token，强制跳转到登陆页
	if (!user) {
		next('/login')
	}
	//否则放行
	next()
})



	// 3. 向外共享路由实例对象
	export default router