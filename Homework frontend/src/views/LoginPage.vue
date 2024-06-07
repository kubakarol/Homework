<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Login</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content>
      <ion-item>
        <ion-label position="floating">Username</ion-label>
        <ion-input v-model="username"></ion-input>
      </ion-item>
      <ion-item>
        <ion-label position="floating">Password</ion-label>
        <ion-input type="password" v-model="password"></ion-input>
      </ion-item>
      <ion-button expand="full" @click="login">Login</ion-button>
      <ion-text color="danger" v-if="errorMessage">{{ errorMessage }}</ion-text>
    </ion-content>
  </ion-page>
</template>

<script>
import axios from 'axios';
import { useStore } from 'vuex';

export default {
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
        const response = await axios.post('https://localhost:7195/api/Auth/login', {
          userName: this.username,
          password: this.password
        });
        this.$store.commit('setToken', response.data.token);
        this.$router.push('/');
      } catch (error) {
        this.errorMessage = 'Invalid username or password';
        console.error(error);
      }
    }
  },
  setup() {
    const store = useStore();
    return { store };
  }
};
</script>
