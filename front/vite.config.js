import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import legacyPlugin from '@vitejs/plugin-legacy'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(),
          legacyPlugin({
              targets:['chrome 52'],  // 需要兼容的目标列表，可以设置多个
          })
  ],
  
  server:{
  	  host:'0.0.0.0',
  	      open: false, //自动打开浏览器
  	      base: "./ ", //生产环境路径
  	      proxy: { // 本地开发环境通过代理实现跨域，生产环境使用 nginx 转发
  	        // 正则表达式写法
  	        '^/api': {
  	          target: 'http://172.22.173.120:10089/api', // 后端服务实际地址
  	          changeOrigin: true, //开启代理
  	          rewrite: (path) => path.replace(/^\/api/, '')
  	        }
  	      }
  },
  
  
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})
