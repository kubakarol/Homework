<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Login</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content class="ion-padding">
      <ion-item class="custom-item">
        <ion-label position="floating" class="custom-label">Username</ion-label>
        <ion-input v-model="username" class="custom-input"></ion-input>
      </ion-item>
      <ion-item class="custom-item">
        <ion-label position="floating" class="custom-label">Password</ion-label>
        <ion-input type="password" v-model="password" class="custom-input"></ion-input>
      </ion-item>
      <ion-button expand="full" @click="login" class="ion-margin-top">Login</ion-button>
      <ion-text color="danger" v-if="errorMessage">{{ errorMessage }}</ion-text>
      <ion-button expand="full" color="secondary" @click="goToRegister" class="ion-margin-top">Register</ion-button>
      <ion-toast :is-open="toast.isOpen" :message="toast.message" :duration="toast.duration" position="top" color="success"></ion-toast>
    </ion-content>
  </ion-page>
</template>

<script>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonItem, IonLabel, IonInput, IonButton, IonText, IonToast } from '@ionic/vue';
import axios from 'axios';
import { useStore } from 'vuex';

export default {
  name: 'Login',
  meta: {
    transitionEl: 'ion-content'
  },
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonItem, IonLabel, IonInput, IonButton, IonText, IonToast },
  data() {
    return {
      username: '',
      password: '',
      errorMessage: '',
      toast: {
        isOpen: false,
        message: '',
        duration: 2000
      }
    };
  },
  methods: {
    async login() {
      try {
        await this.$store.dispatch('login', {
          username: this.username,
          password: this.password
        });
        this.showToast(`Witaj, ${this.username}`);
        this.$router.push('/');
      } catch (error) {
        this.errorMessage = 'Invalid username or password';
        console.error(error);
      }
    },
    goToRegister() {
      this.$router.push('/register');
    },
    showToast(message) {
      this.toast.message = message;
      this.toast.isOpen = true;
      setTimeout(() => {
        this.toast.isOpen = false;
      }, this.toast.duration);
    },
    resetForm() {
      this.username = '';
      this.password = '';
      this.errorMessage = '';
    }
  },
  watch: {
    '$store.state.token'() {
      if (!this.$store.state.token) {
        this.resetForm();
      }
    }
  },
  setup() {
    const store = useStore();
    store.subscribe((mutation, state) => {
      if (mutation.type === 'clearAuthData') {
        store.commit('resetLoginForm');
      }
    });
    return { store };
  }
};
</script>

<style>
.custom-input {
  padding: 7px;
}

.custom-item {
  --highlight-height: 1px;
  --padding-start: 10px;
  --padding-end: 10px;
}

.custom-label {
  --padding-start: 10px;
  --padding-end: 10px;
  color: #666;
  transition: all 0.3s;
}

.custom-item.ion-focused .custom-label,
.custom-item:not(.ion-focused) .custom-label.ion-label-floating {
  transform: scale(0.8) translateY(-20px);
  color: #999;
}

.custom-item.ion-focused .custom-label.ion-label-floating,
.custom-item:not(.ion-focused) .custom-label:not(.ion-label-floating) {
  transform: scale(1) translateY(0);
  color: #000;
}
</style>