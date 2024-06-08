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
        <ion-input v-model="username" class="custom-input"></ion-input>
      </ion-item>
      <ion-item>
        <ion-label position="floating">Password</ion-label>
        <ion-input type="password" v-model="password" class="custom-input"></ion-input>
      </ion-item>
      <ion-button expand="full" @click="login" class="ion-margin-top">Login</ion-button>
      <ion-text color="danger" v-if="errorMessage">{{ errorMessage }}</ion-text>
      <ion-button expand="full" color="secondary" @click="goToRegister" class="ion-margin-top">Register</ion-button>
    </ion-content>
  </ion-page>
</template>

<script>
import {
  IonPage,
  IonHeader,
  IonToolbar,
  IonTitle,
  IonContent,
  IonItem,
  IonLabel,
  IonInput,
  IonButton,
  IonText
} from '@ionic/vue';
import axios from 'axios';
import { useStore } from 'vuex';

export default {
  name: 'Login',
  components: {
    IonPage,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonItem,
    IonLabel,
    IonInput,
    IonButton,
    IonText
  },
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
    },
    goToRegister() {
      this.$router.push('/register');
    }
  },
  setup() {
    const store = useStore();
    return { store };
  }
};
</script>

<style>
.custom-input {
  margin-top: 20px;
}
</style>
