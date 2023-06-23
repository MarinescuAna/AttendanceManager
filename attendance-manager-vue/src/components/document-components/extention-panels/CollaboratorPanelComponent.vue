<template>
  <v-expansion-panel>
    <v-expansion-panel-header class="font-weight-bold"
      >Add new collaborator</v-expansion-panel-header
    >
    <v-expansion-panel-content>
      <validation-observer v-slot="{ handleSubmit, invalid }">
        <v-form @submit.prevent="handleSubmit(doSubmitAsync)">
          <validation-provider
            :rules="rules.email"
            name="email"
            v-slot="{ errors }"
          >
            <v-text-field
              label="Teacher's email"
              v-model="email"
              prepend-icon="mdi-email"
              color="black"
              :error-messages="errors"
              required
            />
          </validation-provider>
          <v-row justify="center" class="pa-8">
            <v-btn
              class="dark_button white--text"
              @click="doSubmitAsync"
              :disabled="invalid"
              >Add new collaborator</v-btn
            >
          </v-row>
        </v-form>
      </validation-observer>
    </v-expansion-panel-content>
  </v-expansion-panel>
</template>
  <script lang="ts">
import Vue from "vue";
import { rules } from "@/plugins/vee-validate";
import AuthService from "@/services/auth.service";
import { Toastification } from "@/plugins/vue-toastification";
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  name: "CollaboratorPanelComponent",
  data: function () {
    return {
      // rules for validating the email
      rules,
      // teacher email
      email: "",
    };
  },
  computed: {
    /**
     * Get the current email address
     */
    currentUserEmail: function (): string {
      return AuthService.getDataFromToken()!.email;
    },
  },
  methods: {
    /**
     * Save the new collaborator
     */
    doSubmitAsync: async function (): Promise<void> {
      if (this.email === this.currentUserEmail) {
        Toastification.simpleError(
          "You are already a member of this document!"
        );
        return;
      }

      const result = await storeHelper.documentStore.addCollaboratorAsync({
        email: this.email,
      });

      if (result) {
        this.email = "";
        Toastification.success("The teacher was added as collaborator!");
      }
    },
  },
});
</script>