
<template>
  <v-container class="mt-2">
    <v-card width="700">
      <v-card-title class="pb-0">
        <h2>Login</h2>
      </v-card-title>
      <v-card-text>
        <validation-observer v-slot="{ handleSubmit, invalid }">
          <v-form @submit.prevent="handleSubmit(doLogin)">
            <validation-provider
              rules="required|email"
              name="email"
              v-slot="{ errors }"
            >
              <v-text-field
                label="Email"
                v-model="email"
                prepend-icon="mdi-email"
                :error-messages="errors"
                required
              />
            </validation-provider>
            <validation-provider name="password" v-slot="{ errors }" rules="required|min:4">
              <v-text-field
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                label="Password"
                prepend-icon="mdi-lock"
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append="showPassword = !showPassword"
                :error-messages="errors"
                required
              />
            </validation-provider>
            <v-btn @click="doLogin" :disabled="invalid" large>Login</v-btn>
          </v-form>
        </validation-observer>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<style lang="scss" scoped>
.align-card {
  flex: 1;
  display: flex;
  align-items: center;
}
</style>

<script lang="ts">
import Vue from "vue";
import UserService from "@/services/auth.service";
import { extend } from "vee-validate";
import { required, email, min } from "vee-validate/dist/rules";

extend("required", {
  ...required,
  message: "{_field_} can not be empty",
});

extend("min", {
  ...min,
  message: "{_field_} must be greater than {length} characters",
});

extend("email", {
  ...email,
  message: "Email must be valid",
});

export default Vue.extend({
  data() {
    return {
      showPassword: false,
      password: "",
      email: "",
    };
  },
  methods: {
    async doLogin() {
      const response = await UserService.login({
        email: this.email,
        password: this.password,
      });

      if (response) {
        this.$router.push("/");
      }
    },
  },
});
</script>
  