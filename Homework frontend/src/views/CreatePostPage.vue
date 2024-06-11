<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Create Post</ion-title>
        <ion-buttons slot="start">
          <ion-back-button></ion-back-button>
        </ion-buttons>
      </ion-toolbar>
    </ion-header>
    <ion-content>
      <ion-list>
        <ion-item>
          <ion-label position="stacked">Title</ion-label>
          <ion-input v-model="title"></ion-input>
        </ion-item>
        <ion-item>
          <ion-label position="stacked">Content</ion-label>
          <ion-textarea v-model="content"></ion-textarea>
        </ion-item>
      </ion-list>
      <ion-button expand="full" @click="submitPost">Submit</ion-button>
      <ion-toast :is-open="toast.isOpen" :message="toast.message" :duration="toast.duration" position="top" color="success"></ion-toast>
    </ion-content>
  </ion-page>
</template>

<script>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel, IonInput, IonTextarea, IonButton, IonToast } from '@ionic/vue';
import axios from 'axios';

export default {
  meta: {
    transitionEl: 'ion-content'
  },
  name: 'CreatePost',
  components: {
    IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel, IonInput, IonTextarea, IonButton, IonToast
  },
  data() {
    return {
      title: '',
      content: '',
      toast: {
        isOpen: false,
        message: '',
        duration: 2000
      }
    };
  },
  computed: {
    userId() {
      return this.$store.state.user ? this.$store.state.user.id : null;
    },
    userName() {
      return this.$store.state.user ? this.$store.state.user.userName : null;
    }
  },
  methods: {
    async submitPost() {
      if (!this.userId || !this.userName) {
        console.error('User ID or UserName not found.');
        return;
      }
      try {
        const postData = {
          title: this.title,
          content: this.content,
          userId: this.userId,
          userName: this.userName
        };
        const response = await axios.post('https://localhost:7195/api/Post/create', postData, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`,
            'Content-Type': 'application/json'
          }
        });
        this.$router.push('/'); // Przekieruj na stronę główną po dodaniu posta
        this.$store.dispatch('fetchPosts'); // Odśwież listę postów po dodaniu nowego posta
        this.title = '';
        this.content = ''; // Clear the form fields after successful submission
        this.showToast('Post successfully added');
      } catch (error) {
        console.error('Error creating post:', error.response ? error.response.data : error.message);
        this.showToast('Error creating post');
      }
    },
    showToast(message) {
      this.toast.message = message;
      this.toast.isOpen = true;
      setTimeout(() => {
        this.toast.isOpen = false;
      }, this.toast.duration);
    }
  }
};
</script>
