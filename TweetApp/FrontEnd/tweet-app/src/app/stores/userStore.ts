import { makeAutoObservable, runInAction } from "mobx";
import { history } from "../..";
import agent from "../api/agent";
import { LoginUser, RegisterUser, User } from "../models/User";
import { store } from "./store";
class UserStore {
	users: User[] = [];
	user: User | null = null;
	loading: boolean = false;
	loadingInitial: boolean = true;
	firstTime: boolean = false;
	constructor() {
		makeAutoObservable(this);
	}

	loadUsers = async () => {
		try {
			const response = await agent.UserRequest.list();
			runInAction(() => {
				response.result.forEach((user) => {
					this.users.push(user);
				});
				this.loadingInitial = false;
			});
		} catch (error) {
			console.log(error);
			runInAction(() => {
				this.loadingInitial = false;
			});
		}
	};

	setLoadingInitial = (state: boolean) => {
		this.loadingInitial = state;
	};

	get isLoggedIn() {
		return !!this.user;
	}

	login = async (credentials: LoginUser) => {
		try {
			const response = await agent.UserRequest.login(credentials);
			if (response.isSuccess) {
				store.commonStore.setToken(response.token!);
				runInAction(() => {
					this.user = response.result;
					//console.log(this.user);
				});
				history.push("/tweets");
				store.modalStore.closeModal();
			} else {
				throw new Error("invalid username or password");
			}
		} catch (error) {
			throw error;
		}
	};

	getUser = async () => {
		try {
			if (this.firstTime || this.loadingInitial) {
				const response = await agent.UserRequest.current();
				if (response.isSuccess) {
					runInAction(() => (this.user = response.result));
				}
			} else {
				this.firstTime = true;
			}
		} catch (error) {
			console.log(error);
		}
	};

	logout = () => {
		store.commonStore.setToken(null);
		window.localStorage.removeItem("jwt");
		this.user = null;
		history.push("/");
	};

	register = async (credentials: RegisterUser) => {
		try {
			const response = await agent.UserRequest.register(credentials);
			if (response.isSuccess) {
				runInAction(() => {
					this.user = response.result;
				});
				var cred: LoginUser = {
					username: response.result.email,
					password: credentials.password,
				};
				this.login(cred);
			} else {
				throw new Error("invalid username or password");
			}
		} catch (error) {
			throw error;
		}
	};
}

export default UserStore;
