import random 
import string 

user_ids = list(range(1,101))
recipient_ids = list(range(1,101))

def generate_message() -> dict:
    # generate random user id 
    random_user_id = random.choice(user_ids)

    # copy the array 
    recipient_ids_copy = recipient_ids.copy()

    # user can't send meesage to himself
    recipient_ids_copy.remove(random_user_id)
    random_recipient_id = random.choice(recipient_ids_copy)

    # gen random msg
    message = ''.join(random.choice(string.ascii_letters) for i in range(32))

    return{
        'user_id': random_user_id,
        'recipient_id': random_recipient_id,
        'message': message
    }

#testing
if __name__ == "__main__":
    print(generate_message())