import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import { IonicVue } from '@ionic/vue';
import '@ionic/vue/css/core.css';

const app = createApp(App)
  .use(IonicVue)
  .use(router)
  .use(store);

router.isReady().then(() => {
  app.mount('#app');
});
