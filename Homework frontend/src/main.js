import { createApp } from 'vue';
import App from './App.vue';
import router from './router/router';
import store from './store/store';
import { IonicVue } from '@ionic/vue';
import { defineCustomElements } from '@ionic/pwa-elements/loader';

// Import Ionic CSS
import '@ionic/vue/css/core.css';
import '@ionic/vue/css/normalize.css';
import '@ionic/vue/css/structure.css';
import '@ionic/vue/css/typography.css';
import '@ionic/vue/css/padding.css';
import '@ionic/vue/css/float-elements.css';
import '@ionic/vue/css/text-alignment.css';
import '@ionic/vue/css/text-transformation.css';
import '@ionic/vue/css/flex-utils.css';
import '@ionic/vue/css/display.css';

// Import Bootstrap CSS
import 'bootstrap/dist/css/bootstrap.css';

// Import Custom Variables
import './theme/variables.css';

// Import Ionic Animations
import { createAnimation } from '@ionic/vue';

store.dispatch('autoLogin');

const app = createApp(App)
  .use(IonicVue, {
    animated: true,
    mode: 'ios', // lub 'md' dla Androida
  })
  .use(router)
  .use(store);

// Define a custom transition animation
router.beforeEach((to, from, next) => {
  const enteringEl = document.querySelector(to.meta.transitionEl);
  const leavingEl = document.querySelector(from.meta.transitionEl);

  if (enteringEl && leavingEl) {
    const animation = createAnimation()
      .addElement(enteringEl)
      .addElement(leavingEl)
      .duration(500)
      .fromTo('opacity', '0', '1');

    animation.play().then(() => {
      next();
    });
  } else {
    next();
  }
});

router.isReady().then(() => {
  app.mount('#app');
  defineCustomElements(window);
});
