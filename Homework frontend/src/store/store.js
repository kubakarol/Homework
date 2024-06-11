import { createStore } from 'vuex';
import axios from 'axios';
import jwt_decode from 'jwt-decode';

const store = createStore({
  state: {
    token: localStorage.getItem('token') || '',
    user: JSON.parse(localStorage.getItem('user')) || null,
    posts: [],
    userPosts: [],
    username: '',
    password: ''
  },
  mutations: {
    setToken(state, token) {
      state.token = token;
      localStorage.setItem('token', token);
      axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    },
    setUser(state, user) {
      state.user = user;
      localStorage.setItem('user', JSON.stringify(user));
    },
    setPosts(state, posts) {
      state.posts = posts;
    },
    setUserPosts(state, userPosts) {
      state.userPosts = userPosts;
    },
    clearAuthData(state) {
      state.token = '';
      state.user = null;
      state.posts = [];
      state.userPosts = [];
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      delete axios.defaults.headers.common['Authorization'];
    },
    resetLoginForm(state) {
      state.username = '';
      state.password = '';
    },
    updatePost(state, post) {
      const index = state.posts.findIndex(p => p.id === post.id);
      if (index !== -1) {
        state.posts[index] = post;
      }
    },
    updateComment(state, comment) {
      const postIndex = state.posts.findIndex(p => p.id === comment.postId);
      if (postIndex !== -1) {
        const commentIndex = state.posts[postIndex].comments.findIndex(c => c.id === comment.id);
        if (commentIndex !== -1) {
          state.posts[postIndex].comments[commentIndex] = comment;
        }
      }
    }
  },
  actions: {
    async login({ dispatch }, authData) {
      try {
        const response = await axios.post('https://localhost:7195/api/Auth/login', {
          userName: authData.username,
          password: authData.password
        });
        const token = response.data.token;
        const decodedToken = jwt_decode(token);
        const user = {
          id: decodedToken.nameid,
          userName: decodedToken.unique_name
        };
        dispatch('setUserAndToken', { user, token });
      } catch (error) {
        console.error('Error during login:', error);
        throw error;
      }
    },
    setUserAndToken({ commit }, { user, token }) {
      commit('setToken', token);
      commit('setUser', user);
    },
    autoLogin({ dispatch }) {
      const token = localStorage.getItem('token');
      if (!token) {
        return;
      }
      const decodedToken = jwt_decode(token);
      const user = {
        id: decodedToken.nameid,
        userName: decodedToken.unique_name
      };
      dispatch('setUserAndToken', { user, token });
    },
    logout({ commit }) {
      commit('clearAuthData');
      commit('resetLoginForm');
    },
    fetchPosts({ commit }) {
      return axios.get('https://localhost:7195/api/Post/getAll')
        .then(response => {
          commit('setPosts', response.data.$values.map(post => ({
            ...post,
            comments: post.comments.$values,
            newComment: ''
          })));
        })
        .catch(error => {
          console.error('Error fetching posts:', error);
        });
    },
    fetchUserPosts({ commit, state }) {
      if (!state.user) return;
      return axios.get(`https://localhost:7195/api/Post/getByUser/${state.user.id}`)
        .then(response => {
          commit('setUserPosts', response.data.$values.map(post => ({
            ...post,
            comments: post.comments.$values,
            newComment: ''
          })));
        })
        .catch(error => {
          console.error('Error fetching user posts:', error);
        });
    },
    async searchPosts({ commit }, query) {
      try {
        const response = await axios.get('https://localhost:7195/api/Post/search', {
          params: {
            query: query
          }
        });
        const posts = response.data.$values.map(post => ({
          ...post,
          comments: post.comments.$values,
          newComment: ''
        }));
        commit('setPosts', posts);
      } catch (error) {
        console.error('Error searching posts:', error);
      }
    },
    async updatePost({ commit }, post) {
      try {
        const response = await axios.put(`https://localhost:7195/api/Post/${post.id}`, post, {
          headers: {
            'Authorization': `Bearer ${this.state.token}`
          }
        });
        commit('updatePost', response.data);
      } catch (error) {
        console.error('Error updating post:', error);
        throw error;
      }
    },
    async updateComment({ commit }, comment) {
      try {
        const response = await axios.put(`https://localhost:7195/api/Post/comment/${comment.id}`, comment, {
          headers: {
            'Authorization': `Bearer ${this.state.token}`
          }
        });
        commit('updateComment', response.data);
      } catch (error) {
        console.error('Error updating comment:', error);
        throw error;
      }
    }
  },
  getters: {
    isAuthenticated(state) {
      return !!state.token;
    },
    getUser(state) {
      return state.user;
    },
    getPosts(state) {
      return state.posts;
    },
    getUserPosts(state) {
      return state.userPosts;
    }
  },
  modules: {}
});

export default store;
