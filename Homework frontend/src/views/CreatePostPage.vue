<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Create Post</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content>
      <ion-item>
        <ion-label position="floating">Title</ion-label>
        <ion-input v-model="title" class="custom-input"></ion-input>
      </ion-item>
      <ion-item>
        <ion-label position="floating">Content</ion-label>
        <ion-input v-model="content" class="custom-input"></ion-input>
      </ion-item>
      <ion-button expand="full" @click="createPost">Create</ion-button>
      <ion-text color="danger" v-if="error">{{ error }}</ion-text>
    </ion-content>
  </ion-page>
</template>

<script>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonButton, IonContent, IonItem, IonLabel, IonInput, IonText } from '@ionic/vue';
import axios from 'axios';
import { useStore } from 'vuex';

export default {
  name: 'CreatePost',
  components: {
    IonPage, IonHeader, IonToolbar, IonTitle, IonButton, IonContent, IonItem, IonLabel, IonInput, IonText
  },
  data() {
    return {
      title: '',
      content: '',
      error: ''
    };
  },
  methods: {
    async createPost() {
      if (!this.title || !this.content) {
        this.error = "Title and content are required.";
        return;
      }

      try {
        const token = this.$store.state.token;
        const response = await axios.post('https://localhost:7195/api/Post/create', {
          title: this.title,
          content: this.content
        }, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        });
        this.$router.push('/');
      } catch (error) {
        console.error('Error creating post:', error);
        if (error.response && error.response.data) {
          this.error = error.response.data.errors ? error.response.data.errors.UserId.join(', ') : error.response.data;
        } else {
          this.error = "Failed to create post. Please try again.";
        }
      }
    },
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
