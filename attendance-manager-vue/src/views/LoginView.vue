
<template>
  <v-container>
    <v-row justify="center">
      <v-card :width="width" class="orange lighten-3">
        <v-card-title class="pa-7">
          <h2>Sign in</h2>
        </v-card-title>
        <v-card-text>
          <validation-observer v-slot="{ handleSubmit, invalid }">
            <v-form @submit.prevent="handleSubmit(doLogin)">
              <validation-provider
                :rules="rules.email"
                name="email"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Email"
                  v-model="email"
                  prepend-icon="mdi-email"
                  :error-messages="errors"
                  :class="padding"
                  required
                />
              </validation-provider>
              <validation-provider
                name="password"
                v-slot="{ errors }"
                :rules="rules.password"
              >
                <v-text-field
                  v-model="password"
                  :type="showPassword ? 'text' : 'password'"
                  label="Password"
                  prepend-icon="mdi-lock"
                  :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                  @click:append="showPassword = !showPassword"
                  :error-messages="errors"
                  required
                  :class="padding"
                />
              </validation-provider>
              <v-row justify="center" class="pa-8">
                <v-btn
                  :width="width"
                  @click="doLogin"
                  :disabled="invalid"
                  large
                  >Sign in</v-btn
                >
              </v-row>
            </v-form>
          </validation-observer>
        </v-card-text>
      </v-card></v-row
    >
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import AuthService from "@/services/auth.service";
import { EventBus } from "@/main";
import { EVENT_BUS_ISLOGGED } from "@/shared/constants";
import {rules} from "@/plugins/vee-validate";

export default Vue.extend({
  name: "LoginView",
  data() {
    return {
      rules,
      // Boolean for hidding or displaying the password
      showPassword: false,
      // Password
      password: "",
      // Email
      email: "",
    };
  },
  computed: {
    /**
     * Get the width according to the device type
     */
    width(): string {
      switch (this.$vuetify.breakpoint.name) {
        case "sm":
        case "md": return "75%";
        case "xs": return "100%";
        default: return "50%";
      }
    },
    /**
     * Get the padding value according to the device type
     */
     padding(): string {
      switch (this.$vuetify.breakpoint.name) {
        case "xs": return "pa-0";
        default: return "pa-6";
      }
    },
  },
  methods: {
    /**
     * Use this method for login: if the login is done, update the navbar and redirect to home page
     */
    async doLogin() {
      const response = await AuthService.login({
        email: this.email,
        password: this.password,
      });

      if (response) {
        EventBus.$emit(EVENT_BUS_ISLOGGED);
        this.$router.push({name:'home'});
      }
    },
  },
});
</script>
  