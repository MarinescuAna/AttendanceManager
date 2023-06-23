
<template>
  <v-container>
    <v-layout align-center column>
      <v-progress-linear
        v-if="showLoading"
        :style="{ width: width }"
        indeterminate
        color="black"
      ></v-progress-linear>
      <v-card :width="width" class="orange_background custom-no-radius">
        <v-card-title class="pa-7">
          <h2>Sign in</h2>
        </v-card-title>
        <v-card-text>
          <validation-observer v-slot="{ handleSubmit, invalid }">
            <v-form @submit.prevent="handleSubmit(doLoginAsync)">
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
                  color="black"
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
                  color="black"
                  :class="padding"
                />
              </validation-provider>
              <v-row justify="center" class="pa-8">
                <v-btn :width="width" class="dark_button white--text" @click="doLoginAsync" :disabled="invalid" large
                  >Sign in</v-btn
                >
              </v-row>
            </v-form>
          </validation-observer>
        </v-card-text>
      </v-card></v-layout
    >
  </v-container>
</template>
<style scoped>
.custom-no-radius {
  border-radius: 0;
}
</style>
<script lang="ts">
import Vue from "vue";
import AuthService from "@/services/auth.service";
import { EventBus } from "@/main";
import { EVENT_BUS_ISLOGGED } from "@/shared/constants";
import { rules } from "@/plugins/vee-validate";

export default Vue.extend({
  name: "LoginView",
  data() {
    return {
      rules,
      showPassword: false,
      password: "",
      email: "",
      showLoading: false,
    };
  },
  computed: {
    /**
     * Get the width according to the device type
     */
    width(): string {
      switch (this.$vuetify.breakpoint.name) {
        case "sm":
        case "md":
          return "75%";
        case "xs":
          return "100%";
        default:
          return "50%";
      }
    },
    /**
     * Get the padding value according to the device type
     */
    padding(): string {
      switch (this.$vuetify.breakpoint.name) {
        case "xs":
          return "pa-0";
        default:
          return "pa-6";
      }
    },
  },
  methods: {
    /**
     * Use this method for login: if the login is done, update the navbar and redirect to home page
     */
    doLoginAsync: async function(): Promise<void> {
      this.showLoading = true;
      const response = await AuthService.loginAsync({
        email: this.email,
        password: this.password,
      });

      if (response) {
        EventBus.$emit(EVENT_BUS_ISLOGGED);
        this.$router.push({ name: "home" });
      }
      this.showLoading = false;
    },
  },
});
</script>
  