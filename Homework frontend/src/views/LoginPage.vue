<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Login</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content class="ion-padding">
      <ion-item>
        <ion-label position="floating">Username</ion-label>
        <ion-input v-model="username" class="ion-padding"></ion-input>
      </ion-item>
      <ion-item>
        <ion-label position="floating">Password</ion-label>
        <ion-input type="password" v-model="password" class="ion-padding"></ion-input>
      </ion-item>
      <ion-button expand="full" @click="login" class="ion-margin-top">Login</ion-button>
      <ion-text color="danger" v-if="errorMessage">{{ errorMessage }}</ion-text>
      <ion-button expand="full" color="secondary" @click="goToRegister" class="ion-margin-top">Register</ion-button>
    </ion-content>
  </ion-page>
</template>

<script>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonItem, IonLabel, IonInput, IonButton, IonText } from '@ionic/vue';
import axios from 'axios';
import { useStore } from 'vuex';

export default {
  name: 'Login',
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonItem, IonLabel, IonInput, IonButton, IonText },
  data() {
    return {
      username: '',
      password: '',
      errorMessage: ''
    };
  },
  methods: {
    async login() {
      try {
        await this.$store.dispatch('login', {
          username: this.username,
          password: this.password
        });
        this.$router.push('/');
      } catch (error) {
        this.errorMessage = 'Invalid username or password';
        console.error(error);
      }
    },
    goToRegister() {
      this.$router.push('/register');
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
  margin-top: 20px;
}
</style>
